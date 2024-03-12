using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SessionStatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI viewersPeakDisplay;
    [SerializeField] TextMeshProUGUI followersChangeDisplay;
    [SerializeField] TextMeshProUGUI followersTotalDisplay;
    [SerializeField] TextMeshProUGUI subsChangeDisplay;
    [SerializeField] TextMeshProUGUI subsTotalDisplay;
    [SerializeField] TextMeshProUGUI donationsCountDisplay;
    [SerializeField] TextMeshProUGUI donationsAmountDisplay;

    void Start(){

        viewersPeakDisplay.text = GameManager.Instance.GetLastSesViewersPeak().ToString();
        followersChangeDisplay.text = GameManager.Instance.GetLastSesFollowersChange().ToString();
        followersTotalDisplay.text = GameManager.Instance.GetCurrentFollowers().ToString();
        subsChangeDisplay.text = GameManager.Instance.GetLastSesSubscribersChange().ToString();
        subsTotalDisplay.text = GameManager.Instance.GetCurrentSubs().ToString();
        donationsCountDisplay.text = GameManager.Instance.GetLastSesDonationsCount().ToString();
        donationsAmountDisplay.text = GameManager.Instance.GetLastSesDonationsAmountTotal().ToString();
    }
}
