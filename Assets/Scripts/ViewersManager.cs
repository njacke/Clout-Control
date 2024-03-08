using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ViewersManager : MonoBehaviour
{
    private NotificationsDisplay notificationsDisplay;
    // VIEWERS
    private Viewer [] viewersPool;
    private List<Viewer> currentViewersList = new();
    private HashSet<int> viewersIndexHash = new();
    private HashSet<int> nonViewersIndexHash = new();
    private float updateViewersCD = 1f;
    private float remainingUpdateViewersCD;

    // INITIAL
    private float subInitialViewChance = 0.8f;
    private float followerInitialViewChance = 0.5f;

    // INTEREST
    private float minViewInterestReq = 0.7f;

    // SATISFACTION
    private float attSatisfactionReq = 0.5f;
    private float minViewSatisfactionReq = -0.1f;
    private float gameSatisfactionFactor = 0.1f;
    private float socialSatisfactionFactor = 0.1f;
    private float updateSatisfationCD = 1f;
    private float remainingUpdateSatisfactionCD;
    
    // FOLLOWERS
    private float minFollowSatisfactionReq = 1f;
    private float updateFollowersCD = 1f;
    private float remainingUpdateFollowersCD;

    // SUBSCRIBERS
    private float minSubSatisfactionReq = 2f;
    private float updateSubscribersCD = 1f;
    private float remainingUpdateSubscribersCD;

    // DONATIONS
    private float minDonationSatisfactionReq = 1f;
    private float donationBaseChance = 0.01f;
    private float donationFollowerChance = 0.02f;
    private float donationSubChance = 0.04f;
    private float donationBaseAmount = 100f;
    private float updateDonationsCD = 2f;
    private float remainingUpdateDonationsCD;

    // MULTIPLIERS
    private float socialActionMultiplier = 1f;
    Dictionary<GameManager.CamSizes, float> camSizeInterestMultipliers = new (){

        {GameManager.CamSizes.Small, 0.5f},
        {GameManager.CamSizes.Medium, 1f},
        {GameManager.CamSizes.Large, 2f}
    };

    void Start(){

        viewersPool = GameManager.Instance.GetViewersPool();
        notificationsDisplay = FindObjectOfType<NotificationsDisplay>();

        // add all indexes of viewers in viewer pool as non viewers [OPTIMISATION EXAMPLE, NOT USED ATM]
        /* for(int i = 0; i < viewersPool.Length; i++){
            nonViewersIndexHash.Add(i);
        } */

        remainingUpdateViewersCD = updateViewersCD;
        remainingUpdateSatisfactionCD = updateSatisfationCD;
        remainingUpdateFollowersCD = updateFollowersCD;
        remainingUpdateSubscribersCD = updateSubscribersCD;
        remainingUpdateDonationsCD = updateDonationsCD;
    }

    void Update(){
        
        // viewers update
        remainingUpdateViewersCD -= Time.deltaTime;
        if(remainingUpdateViewersCD <= 0){
            UpdateViewers();
            remainingUpdateViewersCD = updateViewersCD;
            //Debug.Log("Viewers updated.");
        }

        // satisfaction update
        remainingUpdateSatisfactionCD -= Time.deltaTime;
        if(remainingUpdateSatisfactionCD <= 0){
            UpdateStreamSatisfaction();
            remainingUpdateSatisfactionCD = updateSatisfationCD;
            //Debug.Log("StreamSatisfaction updated.");
        }

        // followers update
        remainingUpdateFollowersCD -= Time.deltaTime;
        if(remainingUpdateFollowersCD <= 0){
            UpdateFollowers();
            remainingUpdateFollowersCD = updateFollowersCD;
            //Debug.Log("Followers updated.");
        }

        // subscribers update
        remainingUpdateSubscribersCD -= Time.deltaTime;
        if(remainingUpdateSubscribersCD <= 0){
            UpdateSubscribers();
            remainingUpdateSubscribersCD = updateSubscribersCD;
            //Debug.Log("Subscribers updated.");
        }

        // donations update
        remainingUpdateDonationsCD -= Time.deltaTime;
        if(remainingUpdateDonationsCD <= 0){
            UpdateDonations();
            remainingUpdateDonationsCD = updateDonationsCD;
            //Debug.Log("Donations updated");
        }        
    }

    // [OPTIMISATION EXAMPLE, NOT USED ATM]
    // add initial viewers from pool of subscribers and (non-sub) followers
    private void SetInitialViewersNew(){

        var subsIndexHash = GameManager.Instance.GetSubsIndexHash();

        // assign new viewers from subscribers
        foreach (int subIndex in subsIndexHash){
            var rollRNG = UnityEngine.Random.Range(0f, 1f); //can optimise with "fake RNG" -> randomly select sample to add 
            if (rollRNG <= subInitialViewChance){
                nonViewersIndexHash.Remove(subIndex);
                viewersIndexHash.Add(subIndex);
            }
        }

        // assign new viewers from (non-sub) followers
        var followersIndexHash = GameManager.Instance.GetFollowersIndexHash();       
        var nonSubFollowersIndexHash = new HashSet<int>(followersIndexHash.Where(x => !subsIndexHash.Contains(x)));

        foreach (int nonSubFollowerIndex in nonSubFollowersIndexHash){
            var rollRNG = UnityEngine.Random.Range(0f, 1f);
            if (rollRNG <= followerInitialViewChance){
                nonViewersIndexHash.Remove(nonSubFollowerIndex);
                viewersIndexHash.Add(nonSubFollowerIndex);
            }
        }

        GameManager.Instance.UpdateCurrentViewers(viewersIndexHash.Count);
    }

    // add initial viewers from pool of subscribers and followers
    private void SetInitialViewers(){

        foreach(Viewer viewer in viewersPool){
            
            var rollRNG = UnityEngine.Random.Range(0f, 1f);

            if(viewer.IsSubscribed && rollRNG <= subInitialViewChance){
                viewer.IsWatching = true;             
                currentViewersList.Add(viewer);
            }

            else if(viewer.IsFollower && rollRNG <= followerInitialViewChance){
                viewer.IsWatching = true;
                currentViewersList.Add(viewer);
            }
        }

        GameManager.Instance.UpdateCurrentViewers(currentViewersList.Count);  
    }

    // [OPTIMISATION EXAMPLE, NOT USED ATM]
    private void UpdateViewersNew(){

        UpdateStreamInterest();

        var newNonViewersIndexHash = new HashSet<int>();
        var newViewerIndexHash = new HashSet<int>();

        var followersIndexHash = GameManager.Instance.GetFollowersIndexHash();
        var nonFollowerViewersIndexHash = new HashSet<int>(viewersIndexHash.Where(x => !followersIndexHash.Contains(x)));

        // LEAVE stream
        foreach (int nonFollowerViewerIndex in viewersIndexHash){ //can optimise by keeping another hash of non viewers with enough interest
            if(viewersPool[nonFollowerViewerIndex].StreamSatisfaction < minViewSatisfactionReq){
                newNonViewersIndexHash.Add(nonFollowerViewerIndex);
                ResetViewerNew(viewersPool[nonFollowerViewerIndex]); //reset interest + satisfaction when leaving stream
            }
            else{
                newViewerIndexHash.Add(nonFollowerViewerIndex);
            }
        }

        // JOIN stream
        foreach (int nonViewerIndex in nonViewersIndexHash){
            if(viewersPool[nonViewerIndex].StreamInterest >= minViewInterestReq){
                newViewerIndexHash.Add(nonViewerIndex);
            }
            else{
                newNonViewersIndexHash.Add(nonViewerIndex);
            }
        }

        nonViewersIndexHash = newNonViewersIndexHash;
        viewersIndexHash = newNonViewersIndexHash;

        GameManager.Instance.UpdateCurrentViewers(viewersIndexHash.Count);
    }

    private void UpdateViewers(){

        UpdateStreamInterest();

        foreach(Viewer viewer in viewersPool){

            // non-followers who's current StreamSatisfaction <= min satisfaction -> LEAVE
            // followers will leave after next UpdateFollowers if they unfollow since -minFollowSatisfaction < minViewSatisfation (can add check if needed)
            if(viewer.IsWatching && !viewer.IsFollower && viewer.StreamSatisfaction < minViewSatisfactionReq){
                ResetViewer(viewer); //reset when leving stream
                currentViewersList.Remove(viewer);
                //Debug.Log("Viewer stopped watching: " + viewer.Name);
            }

            // viewers who's current StreamInterest >= min interest -> JOIN
            else if (!viewer.IsWatching && viewer.StreamInterest >= minViewInterestReq){
                viewer.IsWatching = true;
                currentViewersList.Add(viewer);
                //Debug.Log("New viewer has joined: " + viewer.Name);
            }
        }

        GameManager.Instance.UpdateCurrentViewers(currentViewersList.Count);  
    }

    // user reset used for single stream session when they JOIN or LEAVE stream [OPTIMISATION EXAMPLE, NOT USED ATM]
    private void ResetViewerNew(Viewer viewer){
        viewer.StreamInterest = 0f;
        viewer.StreamSatisfaction = 0f;
    }


    // user reset used for single stream session when they JOIN or LEAVE stream
    private void ResetViewer(Viewer viewer){
        viewer.IsWatching = false;
        viewer.StreamInterest = 0f;
        viewer.StreamSatisfaction = 0f;
    }

    private void UpdateFollowers(){

        int followersChange = 0;

        foreach (Viewer viewer in currentViewersList){
            if (!viewer.IsFollower && viewer.StreamSatisfaction >= minFollowSatisfactionReq){
                viewer.IsFollower = true;
                followersChange++;
                Debug.Log(viewer.Name + " is a new follower.");
            }
            else if(viewer.IsFollower && viewer.StreamSatisfaction < -minFollowSatisfactionReq){
                viewer.IsFollower = false;
                followersChange--;
                Debug.Log(viewer.Name + " is no longer a follower.");
            }
        }

        GameManager.Instance.UpdateCurrentFollowers(followersChange);
    }

    private void UpdateSubscribers(){

        int subscribersChange = 0;

        // viewers can only subscribe; can't unsubscribe
        foreach (Viewer viewer in currentViewersList){
            if (!viewer.IsSubscribed && viewer.StreamSatisfaction >= minSubSatisfactionReq){
                viewer.IsSubscribed = true;
                subscribersChange++;
                notificationsDisplay.UpdateNotifications(viewer.Name, 0, false);
                Debug.Log(viewer.Name + " is a new subscriber.");
            }
        }
        GameManager.Instance.UpdateCurrentSubs(subscribersChange);
    }

    private void UpdateDonations(){
        foreach (Viewer viewer in currentViewersList){
            if (viewer.StreamSatisfaction >= minDonationSatisfactionReq){
                if (viewer.IsSubscribed){
                    HandleDonation(viewer, donationSubChance);
                }
                else if (viewer.IsFollower){
                    HandleDonation(viewer, donationFollowerChance);
                }
                else{
                    HandleDonation(viewer, donationBaseChance);
                }
            }
        }
    }

    private void HandleDonation(Viewer viewer, float donationChance){

        if (UnityEngine.Random.Range(0f, 1f) <= donationChance){
            var donationAmount = (int)Math.Round(donationBaseAmount * UnityEngine.Random.Range(0f, 1f), 0);
            if (donationAmount <= 0){
                donationAmount = 1; // set to 1 so there are no $0 donations
            }

            viewer.DonationsCount++;
            viewer.DonationsTotalSpent += donationAmount;
            GameManager.Instance.UpdateBankroll(donationAmount);
            Debug.Log(viewer.Name + " has donated $" + donationAmount);

            notificationsDisplay.UpdateNotifications(viewer.Name, donationAmount, true);

            var topDLog = GameManager.Instance.GetTopDLog();

            if (topDLog.Any()){
                if (donationAmount > topDLog.Last().Amount){
                    GameManager.Instance.UpdateTopDLog(viewer, donationAmount);
                    Debug.Log("Top D log updated.");
                }
            }
            else{
                GameManager.Instance.UpdateTopDLog(viewer, donationAmount);
            }
        }
    }

    private void UpdateStreamInterest(){

        var currentGameGenre = GameManager.Instance.GetCurrentGameGenre();
        //Debug.Log("Updating StreamInterest; current game genre is " + currentGameGenre);

        var currentCamSize = GameManager.Instance.GetCurrentCamSize();

        foreach (Viewer viewer in viewersPool){
            if (!viewer.IsWatching){
                var rollRNG = UnityEngine.Random.Range(0f, 1f);
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

                //Debug.Log(viewer.Name + " has StreamInterest: " + viewer.StreamInterest);
            }
        }
    }

    private void UpdateStreamSatisfaction(){

        var currentGameGenre = GameManager.Instance.GetCurrentGameGenre();
        var currentCamSize = GameManager.Instance.GetCurrentCamSize();
    
        foreach (Viewer viewer in currentViewersList){

            float gameGenreAffinity = 0f; // default value same as affinity for gameGenre == None

             // find affinity value for current genre
            switch (currentGameGenre){
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

            viewer.StreamSatisfaction += (gameGenreAffinity - attSatisfactionReq) * gameSatisfactionFactor;
            viewer.StreamSatisfaction += (viewer.SocialAffinityAverage - attSatisfactionReq) * socialSatisfactionFactor * camSizeInterestMultipliers[currentCamSize];

            Debug.Log(viewer.Name + " has StreamSatisfaction: " + viewer.StreamSatisfaction);
        }
    }

    public void HandleSocialAction(int socialActionIndex){

        var socialAction = (GameManager.SocialActions)socialActionIndex;
        var currentCamSize = GameManager.Instance.GetCurrentCamSize();

        foreach (Viewer viewer in currentViewersList){
            float socialActionAffinity = 0f;

            // find affinity value for selected action
            switch (socialAction){
                case GameManager.SocialActions.Flirt:
                    socialActionAffinity = viewer.AffinityForFlirt;
                    break;
                case GameManager.SocialActions.Giggle:
                    socialActionAffinity = viewer.AffinityForGiggle;
                    break;
                case GameManager.SocialActions.Hype:
                    socialActionAffinity = viewer.AffinityForHype;
                    break;
                case GameManager.SocialActions.Rage:
                    socialActionAffinity = viewer.AffinityForRage;
                    break;

                default:
                    break;
            }
            
            var satisfactionChange = (socialActionAffinity - attSatisfactionReq) * socialActionMultiplier * camSizeInterestMultipliers[currentCamSize];
            viewer.StreamSatisfaction += satisfactionChange;

            //Debug.Log(viewer.Name + "'s StreamSatisfaction changed " + satisfactionChange + " due to " + socialAction + " social action.");
        }        
    }
}
