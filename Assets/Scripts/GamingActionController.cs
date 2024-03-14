using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class GamingActionController : MonoBehaviour
{
    GameImageController gameImageController;
    ViewersManager viewersManager;
    ChatDisplay chatDisplay;
    private TMP_Dropdown dropdown;
    private Slider slider;
    private bool isOnCD;
    float cooldownDuration = 20f;
    float cooldownRemaining;    
    [SerializeField] AudioClip clickSound;

    void Start(){
        gameImageController = FindObjectOfType<GameImageController>();
        viewersManager = FindObjectOfType<ViewersManager>();
        chatDisplay = FindObjectOfType<ChatDisplay>();

        dropdown = GetComponentInChildren<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(StartAction);

        slider = GetComponentInChildren<Slider>();

        GameManager.Instance.SetCurrentGameGenre(0);
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
        var previousGameGenere = GameManager.Instance.GetCurrentGameGenre();
        GameManager.Instance.SetCurrentGameGenre(value);

        StartCoroutine(gameImageController.ChangeGameImage(previousGameGenere));

        List<Viewer> currentViewersList = viewersManager.GetCurrentViewersList();

        if (currentViewersList.Any() && GameManager.Instance.GetCurrentGameGenre() != GameManager.GameGenres.None){

            var randomIndex = Random.Range(0, currentViewersList.Count);        
            chatDisplay.UpdateChatDisplay(currentViewersList[randomIndex], ChatDisplay.MessageType.GameChange);
        }

        isOnCD = true;
        cooldownRemaining = cooldownDuration;
        dropdown.interactable = false;

        AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);

        Debug.Log("Selected game with index of " + value);
    }
}
