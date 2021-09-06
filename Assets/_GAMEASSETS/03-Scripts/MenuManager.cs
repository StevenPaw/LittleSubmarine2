using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private Animator mainMenuAnimator;
        [SerializeField] private Animator creditsAnimator;
        [SerializeField] private Animator optionsAnimator;
        [SerializeField] private string levelOverviewScene;

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
            SceneManager.LoadScene(levelOverviewScene);
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