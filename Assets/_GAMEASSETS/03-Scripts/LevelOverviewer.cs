using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private TMP_Text moneyText;
        private SaveManager saveManager;

        private void Start()
        {
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
            moneyText.text = saveManager.GetCoins().ToString();
            int completedLevelCount = 0;
            int completedWorlds = 0;
            foreach (int levelStars in saveManager.GetData().levelCompleted)
            {
                Debug.Log("levelStars = " + levelStars);
                if (levelStars > 0)
                {
                    completedLevelCount += 1;
                }

                if (completedLevelCount == 9)
                {
                    completedWorlds += 1;
                    completedLevelCount = 0;
                }
            }
            
            Debug.Log("Completed Worlds: " + completedWorlds);

            if (completedWorlds < worldPanels.Length - 1 && completedWorlds > 0)
            {
                activeWorld = completedWorlds;
            }
            UpdateWorldVisibility();
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

        public void BTN_OpenShop()
        {
            SceneManager.LoadScene(Scenes.DECOSHOP);
        }
        
        public void OnEscape(InputAction.CallbackContext ctx)
        {
            BackToMenu();
        }
    }
}