using System;
using UnityEngine;

namespace LittleSubmarine2
{
    public class LevelButtonManager : MonoBehaviour
    {
        [SerializeField] private int world;
        [SerializeField] private int level;
        private SaveManager saveManager;
        private bool isAvailable = false;
        private UnityEngine.UI.Button button;
        
        private void Start()
        {
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
            button = GetComponent<UnityEngine.UI.Button>();
            isAvailable = GetAvailability();
            button.interactable = isAvailable;
        }

        private bool GetAvailability()
        {
            if ((world * 9) + level > 0)
            {
                bool levels = saveManager.GetData().levelCompleted[(world * 9) + level - 1];
                return levels;
            }
            else
            {
                return true;
            }
        }
    }
}