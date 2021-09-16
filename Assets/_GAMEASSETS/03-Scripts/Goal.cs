using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private GameObject levelCompleteWindow;
        [SerializeField] private string nextLevel;
        [SerializeField] private int world;
        [SerializeField] private int level;
        [SerializeField] private int coinsForCompletion;

        private SaveManager saveManager;

        private void Start()
        {
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameTags.PLAYER))
            {
                levelCompleteWindow.SetActive(true);
                other.GetComponent<PlayerController>().SetCanMove(false);
                saveManager.AddCompletedLevel(world, level);
                saveManager.SaveGame();
            }
        }

        public void OnBackToMenu()
        {
            SceneManager.LoadScene(Scenes.LEVELOVERVIEW);
        }
        
        public void OnNextLevel()
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}