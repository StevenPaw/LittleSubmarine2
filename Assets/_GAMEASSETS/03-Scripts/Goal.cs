using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private GameObject levelCompleteWindow;
        [SerializeField] private string nextLevel;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GAMETAGS.PLAYER))
            {
                levelCompleteWindow.SetActive(true);
                other.GetComponent<PlayerController>().SetCanMove(false);
            }
        }

        public void OnBackToMenu()
        {
            SceneManager.LoadScene(Scenes.LEVELMENU);
        }
        
        public void OnNextLevel()
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}