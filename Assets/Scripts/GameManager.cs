using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // VIEWER POOL
    private string [] viewersNames = {"John", "Ben", "Karl", "Hans", "Smiljan"};
    private Viewer [] viewersPool;
    private float maxAttribute = 1;
    private float minAttribute = 0;
    private bool viewersGenerated = false;

    // SOCIAL
    public enum SocialActions{
        None,
        Flirt,
        Giggle,
        Hype,
        Rage
    }

    SocialActions currentSocialAction = SocialActions.None;

    // GAMES
    public enum GameGenres{
        None,
        RPG,
        Arcade,
        Action,
        Simulation
    }
    
    GameGenres currentGameGenre = GameGenres.None;

    // CAMERA
    public enum CamSizes{
        Small,
        Medium,
        Large,
    }

    CamSizes currentCamSize = CamSizes.Small;

    // COUNTS
    private int currentViewers = 0;
    private int currentFollowers = 0;
    private int currentSubs = 0;
    private int currentBankroll = 0;

    void Awake(){
        if (Instance == null){
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }

        if(!viewersGenerated){
            GenerateViewers();
            viewersGenerated = true;
            Debug.Log("viewersPool length is: " + viewersPool.Length);
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

    public Viewer [] GetViewersPool(){
        return viewersPool;
    }

    public void SetCurrentSocialAction(int socialActionIndex){
        currentSocialAction = (SocialActions)socialActionIndex;
        Debug.Log("currentSocialAction set to: "+ currentSocialAction);
    }

    public SocialActions GetCurrentSocialAction(){
        return currentSocialAction;
    }

    public void SetCurrentGameGenre(int genreIndex){
        currentGameGenre = (GameGenres)genreIndex;
        Debug.Log("currentGameGenre set to: " + currentGameGenre);
    }

    public GameGenres GetCurrentGameGenre(){
        return currentGameGenre;
    }

    public void SetCurrentCamSize(int camSizeIndex){
        currentCamSize = (CamSizes)camSizeIndex;
        Debug.Log("currentCamSize set to: " + currentCamSize);
    }

    public CamSizes GetCurrentCamSize(){
        return currentCamSize;
    }

    public void UpdateCurrentViewers(int newCurrentViewers){
        currentViewers = newCurrentViewers;
        Debug.Log("currentViewers set to: "+ currentViewers);
    }

    public int GetCurrentViewers(){
        return currentViewers;
    }

    public void UpdateCurrentFollowers(int followersChange){
        currentFollowers += followersChange;
        Debug.Log("currentFollowers set to: "+ currentFollowers);
    }

    public int GetCurrentFollowers(){
        return currentFollowers;
    }

    public void UpdateCurrentSubs(int subsChange){
        currentSubs += subsChange;
        Debug.Log("currentSubs set to: "+ currentSubs);
    }

    public int GetCurrentSubs(){
        return currentSubs;
    }

    public void UpdateBankroll(int bankrollChange){
        currentBankroll += bankrollChange;
        Debug.Log("currentBankroll set to: " + currentBankroll);
    }
}
