using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CameraActionController : MonoBehaviour
{
    CameraImageController cameraImageController;
    private TMP_Dropdown dropdown;
    private Slider slider;
    private bool isOnCD;
    private float cooldownDuration = 5f;
    private float cooldownRemaining;
    
    void Start(){
        cameraImageController = FindObjectOfType<CameraImageController>();

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
            dropdown.interactable = true;
            //Debug.Log("Cooldown finished. New action available.");
        }
    }

    public void StartAction(int value){

        GameManager.Instance.SetCurrentCamSize(value);

        StartCoroutine(cameraImageController.ScaleCameraSize());

        isOnCD = true;
        cooldownRemaining = cooldownDuration;
        dropdown.interactable = false;

        Debug.Log("Selected camera with index of " + value);
    }
}

