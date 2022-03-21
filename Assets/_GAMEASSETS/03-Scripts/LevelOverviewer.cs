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
        [SerializeField] private GameObject leftArrow;
        [SerializeField] private GameObject rightArrow;
        [SerializeField] private int activeWorld = 0;
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private LevelObject[] levels;
        [SerializeField] private GameObject[] levelParent;
        [SerializeField] private GameObject levelButtonPrefab;
        private SaveManager saveManager;

        private void Start()
        {
            saveManager = SaveManager.Instance;
            moneyText.text = saveManager.GetCoins().ToString();
            int completedLevelCount = 0; //Exclude the Tutorial World
            foreach (bool levelCompleted in saveManager.GetData().levelCompleted)
            {
                if (levelCompleted)
                {
                    completedLevelCount += 1;
                }
            }
            
            PopulateList();
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
            if (activeWorld == 0)
            {
                leftArrow.SetActive(false);
                rightArrow.SetActive(true);
            }
            else if (activeWorld == worldPanels.Length -1)
            {
                leftArrow.SetActive(true);
                rightArrow.SetActive(false);
            }
            else
            {
                leftArrow.SetActive(true);
                rightArrow.SetActive(true);
            }
            
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
            }
        }

        private void PopulateList()
        {
            List<List<LevelObject>> levelList = new List<List<LevelObject>>();
            foreach (LevelObject lvlObj in levels)
            {
                if (levelList.Count < lvlObj.WorldID +1)
                {
                    levelList.Add(new List<LevelObject>());
                }
                
                
                
                Debug.Log("Levels loaded");
            }

            foreach (LevelObject lvlObj in levels)
            {
                levelList[lvlObj.WorldID].Add(lvlObj);
            }

            foreach (List<LevelObject> lvlList in levelList)
            {
                lvlList.Sort();
            }

            for (int w = 0; w < levelList.Count; w++)
            {
                for (int l = 0; l < levelList[w].Count; l++)
                {
                    LevelButtonManager spawnedLevelButton = Instantiate(levelButtonPrefab, levelParent[w].transform).GetComponent<LevelButtonManager>();
                    spawnedLevelButton.LevelObj = levelList[w][l];
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