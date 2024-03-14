using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI viewersDisplay;
    [SerializeField] TextMeshProUGUI followersDisplay;
    [SerializeField] TextMeshProUGUI subscribersDisplay;
    [SerializeField] TextMeshProUGUI topDName;
    [SerializeField] TextMeshProUGUI topDAmount;

    public void UpdateViewersDisplay(int count){
        viewersDisplay.text = count.ToString();
    }

    public void UpdateFollowersDisplay(int count){
        followersDisplay.text = count.ToString();
    }

    public void UpdateSubscribersDisplay(int count){
        subscribersDisplay.text = count.ToString();
    }

    public void UpdateTopDDisplay(string name, int amount){
        topDName.text = name;
        topDAmount.text = "$" + amount.ToString();

        Debug.Log("UPDATE TOP D DISPLAY CALLES");
    }
}
