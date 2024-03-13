using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class CameraActionController : MonoBehaviour
{
    CameraImageController cameraImageController;
    ViewersManager viewersManager;
    ChatDisplay chatDisplay;

    private TMP_Dropdown dropdown;
    private Slider slider;
    private bool isOnCD;
    private float cooldownDuration = 5f;
    private float cooldownRemaining;

    [SerializeField] AudioClip clickSound;

    
    void Start(){
        cameraImageController = FindObjectOfType<CameraImageController>();
        viewersManager = FindObjectOfType<ViewersManager>();
        chatDisplay = FindObjectOfType<ChatDisplay>();

        dropdown = GetComponentInChildren<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(StartAction);

        slider = GetComponentInChildren<Slider>();
    }

    void Update(){
        if (isOnCD && viewersManager.GetStreamActive()){
            if(cooldownRemaining > 0){
                cooldownRemaining -= Time.deltaTime;
                slider.value = cooldownRemaining / cooldownDuration;
            }
            else{
                isOnCD = false;
                dropdown.interactable = true;
                //Debug.Log("Cooldown finished. New action available.");
            }            
        }
        else if(viewersManager.GetStreamActive()){
            dropdown.interactable = true;
        }
        else{
            dropdown.interactable = false;
        }
    }

    public void StartAction(int value){

        GameManager.Instance.SetCurrentCamSize(value);

        StartCoroutine(cameraImageController.ScaleCameraImageSize());

        // chat msg on camera change to large
        if (GameManager.Instance.GetCurrentCamSize() == GameManager.CamSizes.Large){
            
            List<Viewer> currentViewersList = viewersManager.GetCurrentViewersList();

            if (currentViewersList.Any()){
                var randomIndex = Random.Range(0, currentViewersList.Count);
                chatDisplay.UpdateChatDisplay(currentViewersList[randomIndex], ChatDisplay.MessageType.CameraSizeChange);
            }
        }

        isOnCD = true;
        cooldownRemaining = cooldownDuration;
        dropdown.interactable = false;

        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);

        Debug.Log("Selected camera with index of " + value);
    }
}

