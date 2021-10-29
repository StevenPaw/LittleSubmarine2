using System;
using System.Collections;
using System.Collections.Generic;
using LittleSubmarine2;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlSettingsManager : MonoBehaviour
{
    [SerializeField] private string mainMenuScene;
    [SerializeField] private GameObject steeringWheel;
    [SerializeField] private bool isUsingSteeringWheel;
    [SerializeField] private Toggle usingSteeringWheelToggle;
    private SaveManager saveManager;

    private void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
    }

    public void OnBackButton()
    { 
        
        Debug.Log("Save Settings!");
        //TODO: Save Settings and Stuff
        SceneManager.LoadScene(mainMenuScene);
    }

    public void OnSteeringWheelToggle()
    {
        isUsingSteeringWheel = usingSteeringWheelToggle.isOn;
        steeringWheel.SetActive(isUsingSteeringWheel);
    }
}
