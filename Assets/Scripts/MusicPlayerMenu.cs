using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerMenu : MonoBehaviour
{

    void Awake()
    {
        Destroy(GameObject.Find("MusicPlayerStream"));
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
