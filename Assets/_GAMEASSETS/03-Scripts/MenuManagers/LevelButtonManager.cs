using System;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private Image lockedImage;
        [SerializeField] private TMP_Text textTitle;
        
        private SaveManager saveManager;
        private bool isAvailable;
        private Button button;
        private bool levelCompleted;
        private bool maxMovesCompleted;
        private bool clockCompleted;

        public LevelObject LevelObj
        {
            get => levelObj;
            set => levelObj = value;
        }

        private void Start()
        {
            saveManager = SaveManager.Instance;
            button = GetComponent<Button>();

            emptyImage.DOFade(0f, 0.0f);
            
            levelCompleted = saveManager.GetData().levelCompleted[levelObj.ID];
            maxMovesCompleted = saveManager.GetData().maxMovesCompleted[levelObj.ID];
            clockCompleted = saveManager.GetData().clockCompleted[levelObj.ID];
            if (!levelObj.IsBossLevel)
            {
                textTitle.text = levelObj.ID.ToString();
            }
            else
            {
                textTitle.text = levelObj.BossTitle;
            }
            
            isAvailable = GetAvailability();
            UpdateGoals();
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
            else
            {
                completedImage.DOFade(0f, 0.1f);
            }
            
            if (clockCompleted)
            {
                clockImage.DOFade(1f, 0.5f);
            }
            else
            {
                clockImage.DOFade(0f, 0.1f);
            }

            if (maxMovesCompleted)
            {
                movesImage.DOFade(1f, 0.5f);
            }
            else
            {
                movesImage.DOFade(0f, 0.1f);
            }

            if (levelObj.IsBossLevel)
            {
                if (isAvailable)
                {
                    lockedImage.DOFade(0f, 0.5f);
                    emptyImage.DOFade(1f, 0.5f);
                    textTitle.DOFade(1f, 0.5f);
                }
                else
                {
                    lockedImage.DOFade(1f, 0.5f);
                    emptyImage.DOFade(0f, 0.5f);
                    textTitle.DOFade(0f, 0f);
                }
            }
            else
            {
                if (isAvailable)
                {
                    emptyImage.DOFade(1f, 0.5f);
                }
                else
                {
                    emptyImage.DOFade(0f, 0.1f);
                }
            }
        }

        public void StartLevel()
        {
            if (isAvailable)
            {
                SceneManager.LoadScene("Level-" + levelObj.ID);
            }
        }
    }
}