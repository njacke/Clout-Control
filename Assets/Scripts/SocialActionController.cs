using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SocialActionController : MonoBehaviour
{
    ViewersManager viewersManager;
    private TMP_Dropdown dropdown;
    private Slider slider;
    private bool isOnCD;
    float cooldownDuration = 5f;
    float cooldownRemaining;
    
    void Start(){
        viewersManager = FindObjectOfType<ViewersManager>();

        dropdown = GetComponentInChildren<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(StartAction);

        slider = GetComponentInChildren<Slider>();

    }

    void Update(){
        if (isOnCD && cooldownRemaining > 0){
            cooldownRemaining -= Time.deltaTime;
            slider.value = cooldownRemaining / cooldownDuration;
        }

        else if (isOnCD){            
            isOnCD = false;
            dropdown.value = 0;
            dropdown.interactable = true;
            //Debug.Log("Cooldown finished. New action available.");
        }
    }

    public void StartAction(int value){
        GameManager.Instance.SetCurrentSocialAction(value);

        // if social action is not NONE
        if(value != 0){
            isOnCD = true;
            cooldownRemaining = cooldownDuration;
            dropdown.interactable = false;
            viewersManager.HandleSocialAction(value);

            //actionResetDone = false;

            Debug.Log("Selected social action with index of " + value);
        }
    }
}
