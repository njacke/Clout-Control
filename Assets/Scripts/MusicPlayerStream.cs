using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerStream : MonoBehaviour
{

    void Awake()
    {
        Destroy(GameObject.Find("MusicPlayerMenu"));
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
