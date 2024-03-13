using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class Countdown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sessionEndedAnn;
    [SerializeField] UnityEngine.UI.Image CDImage;
    [SerializeField] AudioClip endStreamSound;
    string sessionEndedAnnText = "Stream session has ended!";
    private float sessionDurationTotal = 60f;
    private float sessionDurationRemaining;
    private float fillAmount;
    bool sessionEndCalled = false;


    void Start(){
        sessionEndedAnn.text = "";
        sessionDurationRemaining = sessionDurationTotal;
    }

    void Update(){
        UpdateCountdown();
    }

    void UpdateCountdown(){
        sessionDurationRemaining -= Time.deltaTime;

        if(sessionDurationRemaining > 0){
            fillAmount = sessionDurationRemaining / sessionDurationTotal;
            CDImage.fillAmount = fillAmount;
        }
        else if(!sessionEndCalled){
            sessionEndedAnn.text = sessionEndedAnnText;

            AudioSource.PlayClipAtPoint(endStreamSound, Camera.main.transform.position);

            StartCoroutine(GameManager.Instance.EndSession());
            sessionEndCalled = true;
        }
    }
}
