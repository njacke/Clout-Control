using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraImageController : MonoBehaviour
{

    private Vector3 smallCameraScale = new Vector3(0.75f, 0.75f, 0.75f);
    private Vector3 mediumCameraScale = new Vector3(1f, 1f, 1f);
    private Vector3 largeCameraScale = new Vector3(1.5f, 1.5f, 1.5f);

    private float transitionDuration = 4f;


    public IEnumerator ScaleCameraSize()
    {
        var targetScale = smallCameraScale;
        float elapsedTime = 0f;

        switch(GameManager.Instance.GetCurrentCamSize()){
            case GameManager.CamSizes.Medium:
                targetScale = mediumCameraScale;
                break;
            case GameManager.CamSizes.Large:
                targetScale = largeCameraScale;
                break;
            
            default:
                break;
        }

        while (elapsedTime < transitionDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }


}