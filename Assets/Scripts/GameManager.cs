using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private SceneLoader sceneLoader;
    private ViewersManager viewersManager;
    private StatsDisplay statsDisplay;
    private float sessionEndDelay = 2f;
    private int streamDay = 0;
    private int maxStreamDay = 3;

    // VIEWER POOL
    private string[] viewersNames = {
        "James", "William", "Robert", "John", "David", "Richard", "Joseph", "Charles", "Thomas", "Christoph",
        "Daniel", "Matthew", "Anthony", "Mark", "Donald", "Steven", "Paul", "Andrew", "Kenneth", "George",
        "Elon", "Kevin", "Brian", "Edward", "Ronald", "Timothy", "Jason", "Larry", "Jeffrey", "Frank",
        "Scott", "Eric", "Stephen", "Raymond", "Gregory", "Joshua", "Dennis", "Jerry", "Walter", "Patrick",
        "Peter", "Harold", "Douglas", "Henry", "Carl", "Arthur", "Ryan", "Roger", "Joe", "Juan",
        "Jack", "Albert", "Jonathan", "Justin", "Terry", "Gerald", "Keith", "Samuel", "Willie", "Ralph",
        "Lawrence", "Nicholas", "Roy", "Benjamin", "Bruce", "Brandon", "Adam", "Harry", "Fred", "Wayne",
        "Billy", "Steve", "Louis", "Jeremy", "Aaron", "Randy", "Howard", "Eugene", "Carlos", "Russell",
        "Mary", "Hillary", "Jennifer", "Linda", "Beth", "Barbara", "Susan", "Jessica", "Sarah", "Karen",
        "Nancy", "Lisa", "Betty", "Dorothy", "Sandra", "Ashley", "Kimberly", "Donna", "Emily", "Michelle"
    };

    private Viewer [] viewersPool;
    private float maxAttribute = 1;
    private float minAttribute = 0;
    private bool viewersGenerated = false;

    // [OPTIMISATION DATA STRUCTURES, NOT USED ATM]
    private HashSet<int> followersIndexHash = new HashSet<int>();
    private HashSet<int> subsIndexHash = new HashSet<int>();
    private Dictionary<int, int> donationsSpentDict = new Dictionary<int, int>();   

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

    // PERM STATS
    private int currentViewersCount = 0;
    private int currentFollowersCount = 0;
    private int currentSubsCount = 0;
    private int currentBankrollAmount = 0;
    private List<TopDLogEntry> topDLog = new List<TopDLogEntry>();

    // TEMP STATS
    private int lastSesViewersPeak = 0;
    private int lastSesFollowersChange = 0;
    private int lastSesSubscribersChange = 0;
    private int lastSesDonationsCount = 0;
    private int lastSesDonationsAmountTotal = 0;

    // OBJECTIVES
    private Dictionary<Objectives.ObjectiveTypes, int> sessionObjectivesDict = new Dictionary<Objectives.ObjectiveTypes, int>();

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

    public int GetStreamDay(){
        return streamDay;
    }

    public int GetMaxStreamDay(){
        return maxStreamDay;
    }

    // VIEWER POOL

    private void GenerateViewers(){

        viewersPool = new Viewer[viewersNames.Length];

        var viewerIndex = 0;

        // create new viewers with all names and random attributes and assign it to array
        foreach(string viewerName in viewersNames){
            
            var newViewer = new Viewer{
                Name = viewerName,

                AffinityForRPG = UnityEngine.Random.Range(minAttribute, maxAttribute),
                AffinityForArcade = UnityEngine.Random.Range(minAttribute, maxAttribute),
                AffinityForAction = UnityEngine.Random.Range(minAttribute, maxAttribute),
                AffinityForSimulation = UnityEngine.Random.Range(minAttribute, maxAttribute),

                AffinityForFlirt = UnityEngine.Random.Range(minAttribute, maxAttribute),
                AffinityForGiggle = UnityEngine.Random.Range(minAttribute, maxAttribute),
                AffinityForHype = UnityEngine.Random.Range(minAttribute, maxAttribute),
                AffinityForRage = UnityEngine.Random.Range(minAttribute, maxAttribute),                
            };

            viewersPool[viewerIndex] = newViewer;
            viewerIndex++;

            //Debug.Log("New viewer " + newViewer.Name + " generated with AfinnityForRPG of " + newViewer.AffinityForRPG);
        }
    }

    public Viewer [] GetViewersPool(){
        return viewersPool;
    }

    // DATA STRUCTURES GET [NOT USED ATM]

    public HashSet<int> GetFollowersIndexHash(){
        return followersIndexHash;
    }

    public HashSet<int> GetSubsIndexHash(){
        return subsIndexHash;
    }
    
    public Dictionary<int, int> GetDonationsSpentDict(){
        return donationsSpentDict;
    }

    // ACTIONS SET & GET

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

    // COUNTS UPDATE & GET

    public void UpdateCurrentViewersCount(int newCurrentViewers){
        currentViewersCount = newCurrentViewers;
        statsDisplay.UpdateViewersDisplay(currentViewersCount);
        //Debug.Log("currentViewers set to: "+ currentViewers);
    }

    public int GetCurrentViewersCount(){
        return currentViewersCount;
    }

    public void UpdateCurrentFollowersCount(int followersChange){
        currentFollowersCount += followersChange;
        statsDisplay.UpdateFollowersDisplay(currentFollowersCount);
        //Debug.Log("currentFollowers set to: "+ currentFollowers);
    }

    public int GetCurrentFollowersCount(){
        return currentFollowersCount;
    }

    public void UpdateCurrentSubsCount(int subsChange){
        currentSubsCount += subsChange;
        statsDisplay.UpdateSubscribersDisplay(currentSubsCount);
        //Debug.Log("currentSubs set to: "+ currentSubs);
    }

    public int GetCurrentSubsCount(){
        return currentSubsCount;
    }

    public void UpdateBankrollAmount(int bankrollChange){
        currentBankrollAmount += bankrollChange;
        //Debug.Log("currentBankroll set to: " + currentBankroll);
    }

    public int GetBankrollAmount(){
        return currentBankrollAmount;
    }

    public class TopDLogEntry{
        public Viewer Viewer { get; set; }
        public int Amount { get; set; }
    }

    public void UpdateTopDLog(Viewer viewer, int donationAmount){
        var topDLogEntry = new TopDLogEntry{
            Viewer = viewer,
            Amount = donationAmount
        };
        
        topDLog.Add(topDLogEntry);
        statsDisplay.UpdateTopDDisplay(viewer.Name, donationAmount);
    }

    public List<TopDLogEntry> GetTopDLog(){
        return topDLog;
    }

    public IEnumerator EndSession(){

        viewersManager.SetStreamActive(false);
        Debug.Log("Stream active set to false");

        lastSesViewersPeak = viewersManager.GetSesViewersPeak();
        lastSesFollowersChange = viewersManager.GetSesFollowersChange();
        lastSesSubscribersChange = viewersManager.GetSesSubscribersChange();
        lastSesDonationsCount = viewersManager.GetSesDonationsCount();
        lastSesDonationsAmountTotal = viewersManager.GetSesDonationsAmountTotal();

        foreach(Viewer viewer in viewersPool){
            viewer.IsWatching = false;
            viewer.StreamInterest = 0f;
            viewer.StreamSatisfaction = 0f;
        }

        yield return new WaitForSeconds(sessionEndDelay);
        
        sceneLoader.LoadSessionStatsScene();
    }

    public void NewSessionReset(){

        statsDisplay = FindObjectOfType<StatsDisplay>();
        viewersManager = FindObjectOfType<ViewersManager>();
        sceneLoader = FindObjectOfType<SceneLoader>();

        statsDisplay.UpdateViewersDisplay(currentViewersCount);
        statsDisplay.UpdateFollowersDisplay(currentFollowersCount);
        statsDisplay.UpdateSubscribersDisplay(currentSubsCount);

        if (topDLog.Any()){
        statsDisplay.UpdateTopDDisplay(topDLog.Last().Viewer.Name, topDLog.Last().Amount);
        }
        else{
        statsDisplay.UpdateTopDDisplay("n/a", 0);
        }

        viewersManager.SetStreamActive(true);

        streamDay++;
    }

    public int GetLastSesViewersPeak(){
        return lastSesViewersPeak;
    }

    public int GetLastSesFollowersChange(){
        return lastSesFollowersChange;
    }

    public int GetLastSesSubscribersChange(){
        return lastSesSubscribersChange;
    }

    public int GetLastSesDonationsCount(){
        return lastSesDonationsCount;
    }

    public int GetLastSesDonationsAmountTotal(){
        return lastSesDonationsAmountTotal;
    }

    public void SetObjectivesDict(Dictionary<Objectives.ObjectiveTypes, int> sesObjectivesDict){
        sessionObjectivesDict = sesObjectivesDict;
    }

    public Dictionary<Objectives.ObjectiveTypes, int> GetObjectivesDict(){
        return sessionObjectivesDict;
    }
}
