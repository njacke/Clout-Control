using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadStreamSessionScene(){
        SceneManager.LoadScene(1);
    }

    public void LoadSessionStatsScene(){
        Debug.Log("Loading session stats scene...");
        SceneManager.LoadScene(2);
    }
}
