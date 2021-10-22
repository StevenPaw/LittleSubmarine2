using System;
using UnityEngine;
using UnityEngine.UI;

namespace LittleSubmarine2
{
    public class LevelButtonManager : MonoBehaviour
    {
        [Header("Level Value")]
        [SerializeField] private int world;
        [SerializeField] private int level;
        
        [Header("StarScore")]
        [SerializeField] private Image[] starImages;
        [SerializeField] private Sprite emptyStar;
        [SerializeField] private Sprite filledStar;
        
        [Header("ClockScore")]
        [SerializeField] private Image clockImage;
        [SerializeField] private Sprite emptyClock;
        [SerializeField] private Sprite filledClock;
        
        private SaveManager saveManager;
        private bool isAvailable = false;
        private UnityEngine.UI.Button button;
        private int stars;
        private bool clock;

        private void Start()
        {
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
            button = GetComponent<UnityEngine.UI.Button>();
            
            foreach (Image img in starImages)
            {
                img.sprite = emptyStar;
            }
            clockImage.sprite = emptyClock;
            
            stars = saveManager.GetData().levelCompleted[(world * 9) + level];
            clock = saveManager.GetData().clockCompleted[(world * 9) + level];
            UpdateStars();
            
            isAvailable = GetAvailability();
            button.interactable = isAvailable;
        }

        private bool GetAvailability()
        {
            if ((world * 9) + level > 0)
            {
                int collectedStarsInLevelBefore = saveManager.GetData().levelCompleted[(world * 9) + level - 1];
                return collectedStarsInLevelBefore > 0;
            }
            else
            {
                return true;
            }
        }

        private void UpdateStars()
        {
            for (int i = 0; i < stars; i++)
            {
                starImages[i].sprite = filledStar;
            }

            if (clock)
            {
                clockImage.sprite = filledClock;
            }
        }
    }
}