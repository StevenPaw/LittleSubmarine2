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
        [SerializeField] private GameObject[] worldPanels;
        [SerializeField] private GameObject[] leftArrows;
        [SerializeField] private GameObject[] rightArrows;
        [SerializeField] private int activeWorld = 0;
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private LevelObject[] levels;
        private SaveManager saveManager;

        private void Start()
        {
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
            moneyText.text = saveManager.GetCoins().ToString();
            int completedLevelCount = 0; //Exclude the Tutorial World
            foreach (bool levelCompleted in saveManager.GetData().levelCompleted)
            {
                if (levelCompleted)
                {
                    completedLevelCount += 1;
                }
            }
            
            UpdateWorldVisibility();
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
            if (SceneManager.GetSceneByName("Level" + "-" + levelToLoad) != null)
            {
                SceneManager.LoadScene("Level" + "-" + levelToLoad);
            }
        }

        public void BTN_OpenShop()
        {
            SceneManager.LoadScene(Scenes.DECOSHOP);
        }
    }
}