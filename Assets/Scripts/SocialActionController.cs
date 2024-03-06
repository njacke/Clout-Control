using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class SocialActionController : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private int activeAction;
    private bool isOnCooldown;
    float cooldownDuration = 5f;
    float cooldownRemaining;
    
    void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(StartAction);
    }

    void Update()
    {
        if (isOnCooldown && cooldownRemaining > 0){
            cooldownRemaining -= Time.deltaTime;
        }

        else{            
            isOnCooldown = false;
            cooldownRemaining = cooldownDuration;
            //Debug.Log("Cooldown finished. New action available.");
        }
    }

    public void StartAction(int value){
        Debug.Log("Selected action with index of " + value);

        if (value == 0){
            isOnCooldown = true;
            Debug.Log("Switching to " + " game");
            Debug.Log("Remaining cooldown is " + cooldownDuration + " seconds");
        }
    }
}
