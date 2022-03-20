using System;
using System.ComponentModel;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LittleSubmarine2
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private GameObject levelCompleteWindow;

        [SerializeField] private GameObject addedCoinsGO;
        [SerializeField] private int coinsPerStar;
        [SerializeField] private TMP_Text coinsAmountText;

        [SerializeField] private TMP_Text movesText;
        [SerializeField] private TMP_Text timeText;

        [SerializeField] private LevelObject levelObj;

        [Header("Stars")] 
        [SerializeField] private Image[] starImage;
        [SerializeField] private Sprite starSpriteFilled;
        [SerializeField] private Sprite starSpriteEmpty;
        
        [Header("Clock")]
        [SerializeField] private Image clockImage;
        [SerializeField] private Sprite clockSpriteFilled;
        [SerializeField] private Sprite clockSpriteEmpty;

        private bool earnedClock;
        private bool earnedMaxMoves;
        private int newCoins;

        private SaveManager saveManager;
        private PlayerController playerController;
        private int usedMoves;
        private TimeSpan usedTime;

        private void Start()
        {
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
            playerController = GameObject.FindGameObjectWithTag(GameTags.PLAYER).GetComponent<PlayerController>();
            levelCompleteWindow.SetActive(false);
            coinsAmountText.text = "+ " + coinsPerStar;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameTags.PLAYER))
            {
                levelCompleteWindow.SetActive(true);
                playerController.SetCanMove(false);
                movesText.text = (playerController.UsedMoves) + " Moves";
                usedTime = DateTime.Now - playerController.StartTime;
                usedMoves = playerController.UsedMoves;
                
                if (usedTime.Hours > 0)
                {
                    timeText.text = "Time: " + usedTime.ToString(@"hh\:mm\:ss");
                }
                else
                {
                    timeText.text = "Time: " + usedTime.ToString(@"mm\:ss");
                }
                
                if (usedMoves <= levelObj.MaxMovesForStar)
                {
                    Debug.Log("UsedMoves: " + usedMoves + " for max (2): " + levelObj.MaxMovesForStar);
                    starImage[1].DOCrossfadeImage(starSpriteFilled, 1.0f);
                    earnedMaxMoves = true;
                }

                if (usedTime.TotalSeconds < levelObj.MaxSecondsForClock)
                {
                    clockImage.DOCrossfadeImage(clockSpriteFilled, 2.0f);
                    earnedClock = true;
                    newCoins += coinsPerStar;
                }

                bool newEarnedCompleted = saveManager.GetData().levelCompleted[levelObj.ID] != true;
                bool newEarnedMaxMoves = saveManager.GetData().maxMovesCompleted[levelObj.ID] != true && earnedMaxMoves;
                bool newEarnedClock = saveManager.GetData().clockCompleted[levelObj.ID] != true && earnedClock;

                if (!newEarnedCompleted && !newEarnedClock)
                {
                    addedCoinsGO.SetActive(false);
                }
                else
                {
                    addedCoinsGO.SetActive(true);
                    coinsAmountText.text = "+ " + newCoins;
                    saveManager.AddCoins(newCoins);
                    saveManager.AddCompletedLevel(levelObj.ID, earnedMaxMoves, earnedClock);
                }
                
                saveManager.SaveGame();
            }
        }

        public void OnBackToMenu()
        {
            SceneManager.LoadScene(Scenes.LEVELOVERVIEW);
        }
        
        public void OnNextLevel()
        {
            SceneManager.LoadScene("Level-" + levelObj.ID + 1);
        }

        public void OnRetryLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}