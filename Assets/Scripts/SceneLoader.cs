using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMenuStartScene(){
        SceneManager.LoadScene(0);
    }

    public void LoadStreamSessionScene(){
        SceneManager.LoadScene(1);
    }

    public void LoadSessionStatsScene(){
        //Debug.Log("Loading session stats scene...");
        SceneManager.LoadScene(2);
    }

    public void LoadObjectivesOrEndScene(){
        if(GameManager.Instance.GetStreamDay() < GameManager.Instance.GetMaxStreamDay()){
            SceneManager.LoadScene(3);
        }
        else{
            Destroy(GameManager.Instance.gameObject);
            SceneManager.LoadScene(4);
        }
    }
}
