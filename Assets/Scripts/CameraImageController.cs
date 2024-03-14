using System.Collections;
using UnityEngine;

public class CameraImageController : MonoBehaviour

{
    UnityEngine.UI.Image cameraImage;

    [SerializeField] Sprite neutralSprite;
    [SerializeField] Sprite flirtSprite;
    [SerializeField] Sprite giggleSprite;
    [SerializeField] Sprite hypeSprite;
    [SerializeField] Sprite rageSprite;
    private Vector3 smallCameraScale = new Vector3(0.75f, 0.75f, 0.75f);
    private Vector3 mediumCameraScale = new Vector3(1f, 1f, 1f);
    private Vector3 largeCameraScale = new Vector3(1.313f, 1.313f, 1.313f);

    private float transitionDuration = 0.5f;

    void Start(){

        cameraImage = GetComponent<UnityEngine.UI.Image>();
        transform.localScale = smallCameraScale;
    }


    public IEnumerator ScaleCameraImageSize()
    {
        var targetScale = smallCameraScale;
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;

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

        while (elapsedTime < transitionDuration){

            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public IEnumerator ChangeCameraImageToAction(float duration){

        var currentSocialAction = GameManager.Instance.GetCurrentSocialAction();

        switch(currentSocialAction){
            case GameManager.SocialActions.Flirt:
                cameraImage.sprite = flirtSprite;
                break;

            case GameManager.SocialActions.Giggle:
                cameraImage.sprite = giggleSprite;
                break;

            case GameManager.SocialActions.Hype:
                cameraImage.sprite = hypeSprite;
                break;

            case GameManager.SocialActions.Rage:
                cameraImage.sprite = rageSprite;
                break;

            default:
                break;            
        }

        yield return new WaitForSeconds(duration);

        cameraImage.sprite = neutralSprite;
    }
}