using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationsDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI notifOne;
    [SerializeField] TextMeshProUGUI notifTwo;
    [SerializeField] TextMeshProUGUI notifThree;
    [SerializeField] TextMeshProUGUI notifFour;


    private void Start() {
        notifOne.text = "";
        notifTwo.text = "";
        notifThree.text = "";
        notifFour.text = "";
    }

    // quick test, fix so i have a log of notifications or smth similar
    public void UpdateNotifications(string name, int donationAmount, bool isDonation){

        notifFour.text = notifThree.text;
        notifThree.text = notifTwo.text;
        notifTwo.text = notifOne.text;     
        
        if(isDonation){
            notifOne.text = name + " has donated $" + donationAmount.ToString() + "!";
        }
        else{
            notifOne.text = name + " has subscribed";
        }
    }
}
