using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ViewersManager : MonoBehaviour
{

    // VIEWER POOL
    private string [] viewersNames = {"John", "Ben", "Karl", "Hans", "Smiljan"};
    private Viewer [] viewersPool;
    private float maxAttribute = 1;
    private float minAttribute = 0;
    private bool viewersGenerated = false;

    // VIEWERS LIST
    private List<Viewer> currentViewersList = new();
    private int currentViewers = 0;
    private float startViewerUpdateCD = 5f;
    private float remainingViewerUpdateCD;

    // INITIAL
    private float subInitialViewChance = 0.8f;
    private float followerInitialViewChance = 0.5f;

    // INTEREST
    private float minViewInterestReq = 0.5f;

    // SATISFACTION
    private float satisfactionAttReq = 0.5f;
    private float minViewSatisfactionReq = 0f;
    private float gameSatisfactionFactor = 0.1f;
    private float socialSatisfactionFactor = 0.1f;
    private float startSatsifactionUpdateCD = 2f;
    private float remainingSatsifactionUpdateCD;

    Dictionary<GameManager.CamSize, float> camSizeInterestMultipliers = new (){
        {GameManager.CamSize.Small, 0.5f},
        {GameManager.CamSize.Medium, 1f},
        {GameManager.CamSize.Large, 2f}
    };

    void Start()
    {
        if(!viewersGenerated){
            GenerateViewers();
            viewersGenerated = true;
            Debug.Log("viewersPool length is: " + viewersPool.Length);
        }

        remainingViewerUpdateCD = startViewerUpdateCD;
        remainingSatsifactionUpdateCD = startSatsifactionUpdateCD;
    }

    void Update()
    {

        // viewers update
        remainingViewerUpdateCD -= Time.deltaTime;
        if(remainingViewerUpdateCD <= 0){
            UpdateViewers();
            remainingViewerUpdateCD = startViewerUpdateCD;
            Debug.Log("Viewers updated.");
        }

        //satisfaction update
        remainingSatsifactionUpdateCD -= Time.deltaTime;
        if(remainingSatsifactionUpdateCD <= 0){
            UpdateStreamSatisfaction();
            remainingSatsifactionUpdateCD = startSatsifactionUpdateCD;
            Debug.Log("StreamSatisfaction updated.");
        }
        
    }

    private void GenerateViewers(){

        viewersPool = new Viewer[viewersNames.Length];

        var viewerIndex = 0;

        // create new viewers with all names and random attributes and assign it to array
        foreach(string viewerName in viewersNames){
            
            var newViewer = new Viewer{
                Name = viewerName,

                AffinityForRPG = Random.Range(minAttribute, maxAttribute),
                AffinityForArcade = Random.Range(minAttribute, maxAttribute),
                AffinityForAction = Random.Range(minAttribute, maxAttribute),
                AffinityForSimulation = Random.Range(minAttribute, maxAttribute),

                AffinityForFlirt = Random.Range(minAttribute, maxAttribute),
                AffinityForGiggle = Random.Range(minAttribute, maxAttribute),
                AffinityForHype = Random.Range(minAttribute, maxAttribute),
                AffinityForRage = Random.Range(minAttribute, maxAttribute),                
            };

            viewersPool[viewerIndex] = newViewer;
            viewerIndex++;

            //Debug.Log("New viewer " + newViewer.Name + " generated with AfinnityForRPG of " + newViewer.AffinityForRPG);
        }
    }

// add initial viewers to current viewer list
    private void UpdateInitialViewers(){
        foreach(Viewer viewer in viewersPool){
            
            var rollRNG = Random.Range(0f, 1f);

            if(viewer.IsSubscribed && rollRNG <= subInitialViewChance){
                currentViewersList.Add(viewer);
            }

            else if(viewer.IsFollowing && rollRNG <= followerInitialViewChance){
                currentViewersList.Add(viewer);
            }
        }
    }

    private void UpdateViewers(){
        UpdateStreamInterest();

        foreach(Viewer viewer in viewersPool){
            if(viewer.IsWatching && viewer.StreamSatisfaction < minViewSatisfactionReq){
                viewer.IsWatching = false;
                viewer.StreamSatisfaction = 0f;
                currentViewersList.Remove(viewer);
                Debug.Log("Viewer stopped watching: " + viewer.Name);
            }
            else if (!viewer.IsWatching && viewer.StreamInterest > minViewInterestReq){
                viewer.IsWatching = true;
                viewer.StreamInterest = 0f;
                currentViewersList.Add(viewer);
                Debug.Log("New viewer has joined: " + viewer.Name);
            }
        }

        SetCurrentViewers();

        // viewers who's current StreamInterest >= min interest -> JOIN
        // viewers who's current StreamSatisfaction <= min satisfaction -> LEAVE
    }

    private void UpdateFollowers(){

    }

    private void UpdateSubscribers(){

    }

    private void UpdateDonations(){

    }

    private void UpdateStreamInterest(){
        var currentGameGenre = GameManager.Instance.GetCurrentGameGenre();
        //Debug.Log("Updating StreamInterest; current game genre is " + currentGameGenre);

        var currentCamSize = GameManager.Instance.GetCurrentCamSize();

        foreach (Viewer viewer in viewersPool){
            if (!viewer.IsWatching){
                var rollRNG = Random.Range(0f, 1f);
                //Debug.Log(viewer.Name + " has rollRNG: " + rollRNG);

                float genreAffinity = 0f;

                // set interest based on game genre being played
                switch (currentGameGenre){
                    case GameManager.GameGenres.RPG:
                        genreAffinity = viewer.AffinityForRPG;
                        break;
                    case GameManager.GameGenres.Arcade:
                        genreAffinity = viewer.AffinityForArcade;
                        break;
                    case GameManager.GameGenres.Action:
                        genreAffinity = viewer.AffinityForAction;
                        break;
                    case GameManager.GameGenres.Simulation:
                        genreAffinity = viewer.AffinityForSimulation;
                        break;
                    default:
                        break;
                }

                if (viewer.StreamInterest < genreAffinity * rollRNG){
                    viewer.StreamInterest = genreAffinity * rollRNG;
                }

                // set interest based on average social affinity & camSize
                var socialInterest = viewer.SocialAffinityAverage * rollRNG * camSizeInterestMultipliers[currentCamSize];

                if (viewer.StreamInterest < socialInterest){
                    viewer.StreamInterest = socialInterest;
                }

                Debug.Log(viewer.Name + " has StreamInterest: " + viewer.StreamInterest);
            }
        }
    }


    private void UpdateStreamSatisfaction(){
        var currentGameGenre = GameManager.Instance.GetCurrentGameGenre();
        var currentCamSize = GameManager.Instance.GetCurrentCamSize();
    
        foreach (Viewer viewer in currentViewersList){

            float gameGenreAffinity = 0f; // default value same as affinity for gameGenre == None

            switch (currentGameGenre)
            {
                case GameManager.GameGenres.RPG:
                    gameGenreAffinity = viewer.AffinityForRPG;
                    break;
                case GameManager.GameGenres.Arcade:
                    gameGenreAffinity = viewer.AffinityForArcade;
                    break;
                case GameManager.GameGenres.Action:
                    gameGenreAffinity = viewer.AffinityForAction;
                    break;
                case GameManager.GameGenres.Simulation:
                    gameGenreAffinity = viewer.AffinityForSimulation;
                    break;

                default:
                    break;
            }

            viewer.StreamSatisfaction += CalculateSatisfaction(gameGenreAffinity, satisfactionAttReq, gameSatisfactionFactor);
            viewer.StreamSatisfaction += CalculateSatisfaction(viewer.SocialAffinityAverage, satisfactionAttReq, socialSatisfactionFactor) * camSizeInterestMultipliers[currentCamSize];

            Debug.Log(viewer.Name + " has StreamSatisfaction: " + viewer.StreamSatisfaction);
        }
    }

    private float CalculateSatisfaction(float affinity, float breakpoint, float factor){
        float result;
        if (affinity > breakpoint){
            result = affinity * factor;
        }
        else{
            result = (1 - affinity) * -factor; // 1- and negative factor to get effect of negative/opposite affinity & satisfaction
        }

        return result;

        //return affinity > breakpoint ? affinity * factor : (1 - affinity) * -factor;
    }

    public void SetCurrentViewers(){
        currentViewers = currentViewersList.Count;
        Debug.Log("Current viewers: " + currentViewers);
    }
}
