using System.Collections;
using System.Collections.Generic;
using LittleSubmarine2;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private float volume;
    [SerializeField] private bool showSteeringWheel;

    public float Volume => volume;
    public bool ShowSteeringWheel => showSteeringWheel;
    
    void Start()
    {
        if (GameObject.FindGameObjectWithTag(GameTags.SETTINGSMANAGER) == this.gameObject)
        {
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("SettingsManager loaded");
        }
        else
        {
            Destroy(this.gameObject);
            Debug.Log("SettingsManager destroyed");
        }
        
        LoadPrefs();
    }

    public void SwitchSteeringWheel(bool isVisible)
    {
        showSteeringWheel = isVisible;

        if (isVisible)
        {
            PlayerPrefs.SetInt("showSteeringWheel", 1);
        }
        else
        {
            PlayerPrefs.SetInt("showSteeringWheel", 0);
        }
    }

    public void SetVolume(float volumeIn)
    {
        volume = volumeIn;
        PlayerPrefs.SetFloat("volume", volume);
    }

    private void LoadPrefs()
    {
        int isVisible = PlayerPrefs.GetInt("showSteeringWheel", 0);
        if (isVisible == 1)
        {
            showSteeringWheel = true;
        }
        else
        {
            showSteeringWheel = false;
        }

        volume = PlayerPrefs.GetFloat("volume", 0.5f);
    }
}
