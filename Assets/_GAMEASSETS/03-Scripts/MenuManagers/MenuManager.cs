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

        [SerializeField] private GameObject DeleteSavesPrompt;

        [SerializeField] private Image submarineBody;
        [SerializeField] private Image submarinePeriscope;
        [SerializeField] private string controlModeScene;

        private SaveManager saveManager;
        private PartManager partManager;

        private void Start()
        {
            saveManager = SaveManager.Instance;
            partManager = PartManager.Instance;
            
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
            SceneManager.LoadScene(saveManager.GetData().levelCompleted[1] ? Scenes.LEVELOVERVIEW : Scenes.TUTORIAL);
        }

        public void BTN_LoadTutorial()
        {
            SceneManager.LoadScene(Scenes.TUTORIAL);
        }

        public void BTN_DeleteSaves()
        {
            DeleteSavesPrompt.SetActive(true);
        }
        
        public void BTN_ControlMode()
        {
            SceneManager.LoadScene(controlModeScene);
        }
        
        public void ClearSaves()
        {
            saveManager.ClearSave();
            DeleteSavesPrompt.SetActive(false);
            Debug.Log("Restarting Game!");

            //Try restarting the game:
            System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe")); //new program
            Application.Quit(); //kill current process
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