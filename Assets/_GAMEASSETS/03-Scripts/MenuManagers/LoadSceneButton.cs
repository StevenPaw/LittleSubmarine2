using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] private string levelToLoad;

        public void LoadScene()
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}