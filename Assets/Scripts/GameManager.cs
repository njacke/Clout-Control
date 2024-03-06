using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // GAMES
    public enum GameGenres{
    None,
    RPG,
    Arcade,
    Action,
    Simulation
    }
    
    GameGenres currentGameGenre;

    // CAMERA
    public enum CamSize{
        Small,
        Medium,
        Large,
    }

    CamSize currentCamSize = CamSize.Small; // declaring for testing purposes


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }    

    public void SetCurrentGameGenre(int genreIndex){
        currentGameGenre = (GameGenres)genreIndex;
        Debug.Log("currentGameGenre set to: " + currentGameGenre);
    }

    public GameGenres GetCurrentGameGenre(){
        return currentGameGenre;
    }

    public void SetCurrentCamSize(int camSizeIndex){
        currentCamSize = (CamSize)camSizeIndex;
        Debug.Log("currentCamSize set to: " + currentCamSize);
    }

    public CamSize GetCurrentCamSize(){
        return currentCamSize;
    }
}
