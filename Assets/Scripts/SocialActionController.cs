using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SocialActionController : MonoBehaviour
{
    private ViewersManager viewersManager;
    private CameraImageController cameraImageController;
    private TMP_Dropdown dropdown;
    private Slider slider;
    private bool isOnCD;
    private float cooldownDuration = 7f;
    private float cooldownRemaining;
    private float actionImageDuration = 2f;

    // SFX
    [SerializeField] AudioClip clickSound;
    [SerializeField] AudioClip flirtSFX;
    [SerializeField] AudioClip giggleSFX;
    [SerializeField] AudioClip hypeSFX;
    [SerializeField] AudioClip rageSFX;

    private Dictionary<GameManager.SocialActions, AudioClip> actionsSFXDict;
    
    void Start(){
        viewersManager = FindObjectOfType<ViewersManager>();
        cameraImageController = FindObjectOfType<CameraImageController>();

        actionsSFXDict = new Dictionary<GameManager.SocialActions, AudioClip>(){
            { GameManager.SocialActions.Flirt, flirtSFX },
            { GameManager.SocialActions.Giggle, giggleSFX },
            { GameManager.SocialActions.Hype, hypeSFX },
            { GameManager.SocialActions.Rage, rageSFX },
        };

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
                dropdown.value = 0;
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
        GameManager.Instance.SetCurrentSocialAction(value);

        var socialAction = (GameManager.SocialActions)value;

        if (socialAction != GameManager.SocialActions.None){
            isOnCD = true;
            cooldownRemaining = cooldownDuration;
            dropdown.interactable = false;
            viewersManager.HandleSocialAction(value);

            StartCoroutine(cameraImageController.ChangeCameraImageToAction(actionImageDuration));
            
            AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
            AudioSource.PlayClipAtPoint(actionsSFXDict[socialAction], Camera.main.transform.position);

            Debug.Log("Selected social action with index of " + value);
        }
    }
}
