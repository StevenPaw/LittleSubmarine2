using System;
using UnityEngine;

namespace LittleSubmarine2
{
    public class LevelButtonManager : MonoBehaviour
    {
        [SerializeField] private int world;
        [SerializeField] private int level;
        private ActiveSaveData activeSaveData;
        private bool isAvailable = false;
        private UnityEngine.UI.Button button;
        
        private void Start()
        {
            activeSaveData = GameObject.FindGameObjectWithTag(GameTags.ACTIVESAVEDATA).GetComponent<ActiveSaveData>();
            button = GetComponent<UnityEngine.UI.Button>();
            isAvailable = GetAvailability();
            button.interactable = isAvailable;
        }

        private bool GetAvailability()
        {
            if ((world * 9) + level > 1)
            {
                bool levels = activeSaveData.GetData().levelCompleted[(world * 9) + level - 1];
                return levels;
            }
            else
            {
                return true;
            }
        }
    }
}