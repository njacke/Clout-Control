using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor;
using Unity.VisualScripting;

public class Objectives : MonoBehaviour
{

    // SESSION OBJECTIVES
    [SerializeField] TextMeshProUGUI objectiveOneDisplay;
    [SerializeField] TextMeshProUGUI objectiveTwoDisplay;

    private int objectivesCount = 2;

    // MODIFIERS + MINS
    private float viewersPeakModifier = 0.1f;
    private int viewersPeakIncreaseMin = 5;
    private float followersTotalModifier = 0.1f;
    private int followersTotalIncreaseMin = 3;
    private float subsTotalModifier = 0.1f;
    private int subsTotalIncreaseMin = 1;
    private float donationAmountModifier = 0.1f;
    private int donationAmountIncreaseMin = 10;

    public enum ObjectiveTypes{
        ViewersPeak,
        FollowersTotal,
        SubsTotal,
        DonationsAmount,
    }

    private Dictionary<ObjectiveTypes, string> objectivesDisplayDict= new Dictionary<ObjectiveTypes, string>(){
        { ObjectiveTypes.ViewersPeak, "Viewers Peak: " },
        { ObjectiveTypes.FollowersTotal, "Followers Total: " },
        { ObjectiveTypes.SubsTotal, "Subscriptions Total: "},
        { ObjectiveTypes.DonationsAmount, "Donations Amount: $"}
    };

    void Start(){

        var sesObjectivesDict = GenerateObjectives();
        DisplayObjectives(sesObjectivesDict);    
        GameManager.Instance.SetObjectivesDict(sesObjectivesDict);
    }


    private Dictionary<ObjectiveTypes, int> GenerateObjectives(){

        var selectedObjectiveTypes = new List<ObjectiveTypes>();

        while(selectedObjectiveTypes.Count < objectivesCount){

            ObjectiveTypes randomObjective = (ObjectiveTypes)UnityEngine.Random.Range(0, objectivesDisplayDict.Count);

            if(!selectedObjectiveTypes.Contains(randomObjective)){
                selectedObjectiveTypes.Add(randomObjective);
            }
        }

        var newSesObjectivesDict = new Dictionary<ObjectiveTypes, int>();

        foreach(ObjectiveTypes selectedObjectiveType in selectedObjectiveTypes){
            
            int goalValue = 0;

            switch(selectedObjectiveType){
                case ObjectiveTypes.ViewersPeak:
                    goalValue = GenerateObjectiveGoal(GameManager.Instance.GetLastSesViewersPeak(), viewersPeakModifier, viewersPeakIncreaseMin);
                    break;

                case ObjectiveTypes.FollowersTotal:                    
                    goalValue = GenerateObjectiveGoal(GameManager.Instance.GetCurrentFollowersCount(), followersTotalModifier, followersTotalIncreaseMin);
                    break;

                case ObjectiveTypes.SubsTotal:
                    goalValue = GenerateObjectiveGoal(GameManager.Instance.GetCurrentSubsCount(), subsTotalModifier, subsTotalIncreaseMin);
                    break;

                case ObjectiveTypes.DonationsAmount:
                goalValue = GenerateObjectiveGoal(GameManager.Instance.GetLastSesDonationsAmountTotal(), donationAmountModifier, donationAmountIncreaseMin);
                    break;

                default:
                    break;                
            }

            newSesObjectivesDict.Add(selectedObjectiveType, goalValue);
        }

        return newSesObjectivesDict;
    }

    public int GenerateObjectiveGoal(int baseValue, float modifier, int minIncrease){
        var goal = baseValue * (modifier + 1);
        var goalRounded = (int)Math.Round(goal, 0);

        if (goalRounded < minIncrease){
            goalRounded = minIncrease;
        }

        return goalRounded;
    }

    private void DisplayObjectives(Dictionary<ObjectiveTypes, int> objectivesDict){

        var objectivesDisplayList = new List<string>();

        foreach(KeyValuePair<ObjectiveTypes, int> objective in objectivesDict){
            var objectiveDisplay = objectivesDisplayDict[objective.Key] + objective.Value.ToString();
            objectivesDisplayList.Add(objectiveDisplay);
        }

        // [TO-DO] initialise a list with display.text if scope gets bigger
        if(objectivesDisplayList.Count == objectivesCount){
            objectiveOneDisplay.text = objectivesDisplayList[0];
            objectiveTwoDisplay.text = objectivesDisplayList[1];
        }
    }
}
