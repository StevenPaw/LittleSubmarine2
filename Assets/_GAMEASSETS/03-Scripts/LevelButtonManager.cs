using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LittleSubmarine2
{
    public class LevelButtonManager : MonoBehaviour
    {
        [Header("Level Value")] 
        [SerializeField] private LevelObject levelObj;
        
        [Header("Score")]
        [SerializeField] private Image emptyImage;
        [SerializeField] private Image completedImage;
        [SerializeField] private Image clockImage;
        [SerializeField] private Image movesImage;
        
        private SaveManager saveManager;
        private bool isAvailable;
        private Button button;
        private bool levelCompleted;
        private bool maxMovesCompleted;
        private bool clockCompleted;

        private void Start()
        {
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
            button = GetComponent<Button>();

            emptyImage.DOFade(0f, 0.0f);
            emptyImage.DOFade(0f, 0.0f);
            emptyImage.DOFade(0f, 0.0f);
            emptyImage.DOFade(0f, 0.0f);
            
            levelCompleted = saveManager.GetData().levelCompleted[levelObj.ID];
            maxMovesCompleted = saveManager.GetData().maxMovesCompleted[levelObj.ID];
            clockCompleted = saveManager.GetData().clockCompleted[levelObj.ID];
            UpdateGoals();
            
            isAvailable = GetAvailability();
            button.interactable = isAvailable;
        }

        private bool GetAvailability()
        {
            if (levelObj.ID > 0)
            {
                return saveManager.GetData().levelCompleted[levelObj.ID - 1];
            }
            return true;
        }

        private void UpdateGoals()
        {
            if (levelCompleted)
            {
                completedImage.DOFade(1f, 0.5f);
            }
            
            if (clockCompleted)
            {
                clockImage.DOFade(1f, 0.5f);
            }

            if (maxMovesCompleted)
            {
                movesImage.DOFade(1f, 0.5f);
            }
        }
    }
}