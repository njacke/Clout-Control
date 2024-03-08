using System.Collections;
using UnityEngine;

public class GameImageController : MonoBehaviour
{  
    private Coroutine scalingCoroutine;
    private Vector3 minGameScale = new Vector3(0f, 0f, 0f);
    private Vector3 maxGameScale = new Vector3(1f, 1f, 1f);
    private float transitionDuration = 1f;

    [SerializeField] Sprite RPGImage;
    [SerializeField] Sprite arcadeSprite;
    [SerializeField] Sprite actionSprite;
    [SerializeField] Sprite simulationSprite;

    void Start(){
        transform.localScale = minGameScale;
    }

    public IEnumerator ChangeGameImage(GameManager.GameGenres previousGameGenere){
        
        Sprite newSprite = null;

        switch(GameManager.Instance.GetCurrentGameGenre()){
            case GameManager.GameGenres.RPG:
                newSprite = RPGImage;
                break;
            case GameManager.GameGenres.Arcade:
                newSprite = arcadeSprite;
                break;
            case GameManager.GameGenres.Action:
                newSprite = actionSprite;
                break;
            case GameManager.GameGenres.Simulation:
                newSprite = simulationSprite;
                break;

            default:
                break;
        }

        if (scalingCoroutine != null){
            StopCoroutine(scalingCoroutine);
        }

        if(previousGameGenere != GameManager.GameGenres.None){
            scalingCoroutine = StartCoroutine(ScaleGameImageSize(minGameScale));
            yield return scalingCoroutine;
        }

        if(newSprite != null){
            UnityEngine.UI.Image currentImage = GetComponent<UnityEngine.UI.Image>();
            currentImage.sprite = newSprite;
            scalingCoroutine = StartCoroutine(ScaleGameImageSize(maxGameScale));
            yield return scalingCoroutine;
        }
    }


    public IEnumerator ScaleGameImageSize(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsedTime < transitionDuration){

            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    } 
}
