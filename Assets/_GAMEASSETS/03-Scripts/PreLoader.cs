using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LittleSubmarine2
{
    /// <summary>
    /// Script for the preloader that hides the fact, that different scripts are loaded in the background
    /// </summary>
    public class PreLoader : MonoBehaviour
    {
        [SerializeField] private float fadeDuration; //how long the fading would need
        [SerializeField] private float fadeDelay; //how long the preloader should wait before fading
        [SerializeField] private string preloaderSceneName; //the name of the preloader-Scene
        [SerializeField] private string sceneToLoadAfterPreload; //the name of the scene that should be loaded after the preloader
        [SerializeField] private Image blackBGRenderer; //A reference to the renderer of the black background

        [SerializeField] private GameObject lunasnailLogo; //The GO that holds the LunaSnail Logo
        [SerializeField] private float logoStartSize; //how large the logo should be at the beginning
        [SerializeField] private float logoEndSize; //how large the logo should be at the end
        [SerializeField] private float logoAnimationTime; //how long the animation should take
        [SerializeField] private GameObject blackBG;

        private void Start()
        {
            SceneManager.LoadSceneAsync(sceneToLoadAfterPreload, LoadSceneMode.Additive); //Load Scene after preload
            blackBGRenderer.DOFade(0, fadeDuration).SetDelay(fadeDelay).OnComplete(ClosePreloader); //Fadeout the black Background

            lunasnailLogo.transform.localScale = Vector3.one * logoStartSize;
            
            //Do the animation
            Sequence logoAnimation = DOTween.Sequence();
            logoAnimation.Append(lunasnailLogo.GetComponent<Image>().DOFade(1, logoAnimationTime / 3));
            logoAnimation.Insert(0,lunasnailLogo.transform.DOScale(new Vector3(logoEndSize, logoEndSize), logoAnimationTime));
            logoAnimation.Insert(logoAnimationTime * 0.6f,lunasnailLogo.GetComponent<Image>().DOFade(0, logoAnimationTime / 3));
        }

        /// <summary>
        /// Unloads the preloader
        /// </summary>
        private void ClosePreloader()
        {
            Debug.Log("Closing Preloader");
            blackBG.SetActive(false);
            SceneManager.UnloadSceneAsync(preloaderSceneName);
        }
    }
}