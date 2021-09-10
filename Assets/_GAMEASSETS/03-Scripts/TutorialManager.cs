using System;
using System.Collections;
using System.Collections.Generic;
using LittleSubmarine2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject controlSelectBox;
    [SerializeField] private GameObject controlSelectBackground;
    [SerializeField] private GameObject tutorialBox;
    [SerializeField] private Image[] arrowButtons;
    [SerializeField] private Image backButton;
    [SerializeField] private Image pauseButton;
    [SerializeField] private TMP_Text tutorialText;

    [SerializeField] private int tutorialStep;
    [SerializeField] private PlayerController playerController;
    private SettingsManager settings;

    private void Start()
    {
        playerController.SetCanMove(false);
        settings = GameObject.FindGameObjectWithTag(GameTags.SETTINGSMANAGER).GetComponent<SettingsManager>();
        
        controlSelectBox.SetActive(true);
        controlSelectBackground.SetActive(true);
        tutorialBox.SetActive(false);
    }

    public void SetControlsSteeringWheel()
    {
        settings.SwitchSteeringWheel(true);
        AdvanceTutorialSteps(1);
    }
    public void SetControlsSwipe()
    {
        settings.SwitchSteeringWheel(false);
        AdvanceTutorialSteps(1);
    }

    public void AdvanceTutorialSteps(int stepIn)
    {
        tutorialStep = stepIn;
        switch (tutorialStep)
        {
            case 1:
                controlSelectBox.SetActive(false);
                controlSelectBackground.SetActive(false);
                tutorialBox.SetActive(true);
                playerController.UpdateArrowButtons();
                playerController.SetCanMove(true);
                //Move Tutorial
                if (settings.ShowSteeringWheel)
                {
                    tutorialText.text = "Use the arrows to move your submarine";
                    foreach (Image img in arrowButtons)
                    {
                        img.color = Color.red;
                    }
                }
                else
                {
                    tutorialText.text = "Swipe over the screen to move your submarine";
                }

                break;
            case 2:
                //Push Tutorial
                if (settings.ShowSteeringWheel)
                {
                    foreach (Image img in arrowButtons)
                    {
                        img.color = Color.white;
                    }
                }
                backButton.color = Color.white;
                tutorialText.text = "You can push fishes";
                break;
            case 3:
                //Undo Tutorial
                tutorialText.text = "Press the undo button to reverse a step";
                backButton.color = Color.red;
                pauseButton.color = Color.white;
                break;
            case 4:
                //Pause Tutorial
                tutorialText.text = "Press the pause button to pause";
                backButton.color = Color.white;
                pauseButton.color = Color.red;
                break;
            case 5:
                //Button Tutorial
                tutorialText.text = "Push fishes of same color on buttons to open gates";
                pauseButton.color = Color.white;
                break;
            case 6:
                //Have fun Tutorial
                tutorialText.text = "Have fun!";
                tutorialBox.SetActive(true);
                break;
            case 7:
                tutorialBox.SetActive(false);
                break;
        }
    }
}
