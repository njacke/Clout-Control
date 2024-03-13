using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionStatsDisplay : MonoBehaviour
{
    // STATS
    [SerializeField] TextMeshProUGUI viewersPeakDisplay;
    [SerializeField] TextMeshProUGUI followersChangeDisplay;
    [SerializeField] TextMeshProUGUI followersTotalDisplay;
    [SerializeField] TextMeshProUGUI subsChangeDisplay;
    [SerializeField] TextMeshProUGUI subsTotalDisplay;
    [SerializeField] TextMeshProUGUI donationsCountDisplay;
    [SerializeField] TextMeshProUGUI donationsAmountDisplay;

    // OBJECTIVES
    [SerializeField] UnityEngine.UI.Image viewerObjImage;
    [SerializeField] UnityEngine.UI.Image followObjImage;
    [SerializeField] UnityEngine.UI.Image subObjImage;
    [SerializeField] UnityEngine.UI.Image donationObjImage;
    private bool viewerObjCompleted = false;
    private bool followObjCompleted = false;
    private bool subObjCompleted = false;
    private bool donationObjCompleted = false;



    void Start(){

        SetDisplayTexts();
        SetObjectivesCompletion();
        SetObjectivesCheckmark();
    }

    public void SetDisplayTexts(){

        viewersPeakDisplay.text = GameManager.Instance.GetLastSesViewersPeak().ToString();
        followersChangeDisplay.text = GameManager.Instance.GetLastSesFollowersChange().ToString();
        followersTotalDisplay.text = GameManager.Instance.GetCurrentFollowersCount().ToString();
        subsChangeDisplay.text = GameManager.Instance.GetLastSesSubscribersChange().ToString();
        subsTotalDisplay.text = GameManager.Instance.GetCurrentSubsCount().ToString();
        donationsCountDisplay.text = GameManager.Instance.GetLastSesDonationsCount().ToString();
        donationsAmountDisplay.text = "$" + GameManager.Instance.GetLastSesDonationsAmountTotal().ToString();
    }


    public void SetObjectivesCheckmark(){

        // [TO-DO] initialise a list with all image + bool object if scope gets bigger
        viewerObjImage.enabled = viewerObjCompleted;
        followObjImage.enabled = followObjCompleted;
        subObjImage.enabled = subObjCompleted;
        donationObjImage.enabled = donationObjCompleted;
    }

    public void SetObjectivesCompletion(){
        
        var sesObjectivesDict = GameManager.Instance.GetObjectivesDict();

        foreach(KeyValuePair<Objectives.ObjectiveTypes, int> sesObjective in sesObjectivesDict){
            
            switch(sesObjective.Key){
                case Objectives.ObjectiveTypes.ViewersPeak:
                    if(GameManager.Instance.GetLastSesViewersPeak() >= sesObjective.Value){
                        viewerObjCompleted = true;
                    }
                    break;

                case Objectives.ObjectiveTypes.FollowersTotal:
                    if(GameManager.Instance.GetCurrentFollowersCount() >= sesObjective.Value){
                        followObjCompleted = true;
                    }
                    break;

                case Objectives.ObjectiveTypes.SubsTotal:
                    if(GameManager.Instance.GetCurrentSubsCount() >= sesObjective.Value){
                        subObjCompleted = true;
                    }
                    break;

                case Objectives.ObjectiveTypes.DonationsAmount:
                    if(GameManager.Instance.GetLastSesDonationsAmountTotal() >= sesObjective.Value){
                        donationObjCompleted = true;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
