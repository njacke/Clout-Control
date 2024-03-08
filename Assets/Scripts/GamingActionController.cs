using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamingActionController : MonoBehaviour
{
    GameImageController gameImageController;
    private TMP_Dropdown dropdown;
    private Slider slider;
    private bool isOnCD;
    float cooldownDuration = 10f;
    float cooldownRemaining;
    
    void Start(){
        gameImageController = FindObjectOfType<GameImageController>();

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
        var previousGameGenere = GameManager.Instance.GetCurrentGameGenre();
        GameManager.Instance.SetCurrentGameGenre(value);

        StartCoroutine(gameImageController.ChangeGameImage(previousGameGenere));

        isOnCD = true;
        cooldownRemaining = cooldownDuration;
        dropdown.interactable = false;

        Debug.Log("Selected game with index of " + value);
    }
}
