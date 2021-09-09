using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class LevelOverviewer : MonoBehaviour
    {
        [SerializeField] private string mainMenuSceneName;
        [SerializeField] private GameObject[] worldPanels;
        [SerializeField] private GameObject[] leftArrows;
        [SerializeField] private GameObject[] rightArrows;
        [SerializeField] private int activeWorld = 0;

        private void Start()
        {
            UpdateWorldVisibility();
        }

        public void OnEscape(InputAction.CallbackContext ctx)
        {
            BackToMenu();
        }
        
        public void BackToMenu()
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }

        public void IncreaseWorldIndex()
        {
            if (activeWorld < worldPanels.Length)
            {
                activeWorld += 1;
            }
            UpdateWorldVisibility();
        }
        
        public void DecreaseWorldIndex()
        {
            if (activeWorld > 0)
            {
                activeWorld -= 1;
            }
            UpdateWorldVisibility();
        }

        private void UpdateWorldVisibility()
        {
            for (int i = 0; i < worldPanels.Length; i++)
            {
                if (i != activeWorld)
                {
                    worldPanels[i].SetActive(false);
                }
                else
                {
                    worldPanels[i].SetActive(true);
                }

                if (i == 0)
                {
                    leftArrows[i].SetActive(false);
                }

                if (i == worldPanels.Length - 1)
                {
                    rightArrows[i].SetActive(false);
                }
            }
        }

        public void StartLevel(int levelToLoad)
        {
            if (SceneManager.GetSceneByName("Level" + activeWorld + "-" + levelToLoad) != null)
            {
                SceneManager.LoadScene("Level" + activeWorld + "-" + levelToLoad);
            }
        }
    }
}