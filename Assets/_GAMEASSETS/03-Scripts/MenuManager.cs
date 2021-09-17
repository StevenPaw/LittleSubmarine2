using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LittleSubmarine2
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Animator mainMenuAnimator;
        [SerializeField] private Animator creditsAnimator;
        [SerializeField] private Animator optionsAnimator;
        [SerializeField] private string levelOverviewScene;
        [SerializeField] private string tutorialLevel;

        [SerializeField] private Image submarineBody;
        [SerializeField] private Image submarinePeriscope;
        
        private SaveManager saveManager;
        private PartManager partManager;

        private void Start()
        {
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
            partManager = GameObject.FindGameObjectWithTag(GameTags.PARTMANAGER).GetComponent<PartManager>();
            
            submarineBody.sprite = partManager.GetBodyByID(saveManager.GetData().selectedBody).SpriteImage;
            submarinePeriscope.sprite = partManager.GetPeriscopeByID(saveManager.GetData().selectedPeriscope).SpriteImage;
        }

        public void ToMenu()
        {
            mainMenuAnimator.SetBool("isVisible", true);
            creditsAnimator.SetBool("isVisible", false);
            optionsAnimator.SetBool("isVisible", false);
        }

        public void ToCredits()
        {
            mainMenuAnimator.SetBool("isVisible", false);
            creditsAnimator.SetBool("isVisible", true);
            optionsAnimator.SetBool("isVisible", false);
        }

        public void ToOptions()
        {
            mainMenuAnimator.SetBool("isVisible", false);
            creditsAnimator.SetBool("isVisible", false);
            optionsAnimator.SetBool("isVisible", true);
        }

        public void ToLevelOverview()
        {
            if (saveManager.GetData().levelCompleted[0] > 0)
            {
                SceneManager.LoadScene(levelOverviewScene);
            }
            else
            {
                SceneManager.LoadScene(tutorialLevel);
            }
        }

        public void ClearSaves()
        {
            saveManager.ClearSave();
        }

        public void CloseGame()
        {
            Debug.Log("Game quitting!");
            Application.Quit();
        }
        
        public void OnEscapeQuit(InputAction.CallbackContext ctx)
        {
            CloseGame();
        }
    }
}