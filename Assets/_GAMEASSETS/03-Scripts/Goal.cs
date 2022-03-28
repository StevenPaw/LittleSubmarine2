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
        [SerializeField] private Image[] goalsImage;

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
                goalsImage[0].DOFade(1f, 1f).SetDelay(0.5f);

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
                    goalsImage[1].DOFade(1f, 1f).SetDelay(1f);
                    earnedMaxMoves = true;
                }

                if (usedTime.TotalSeconds < levelObj.MaxSecondsForClock)
                {
                    goalsImage[2].DOFade(1f, 1f).SetDelay(1.5f);
                    earnedClock = true;
                    newCoins += coinsPerStar;
                }

                bool newEarnedCompleted = saveManager.GetData().levelCompleted[levelObj.ID] != true;
                bool newEarnedMaxMoves = saveManager.GetData().maxMovesCompleted[levelObj.ID] != true && earnedMaxMoves;
                bool newEarnedClock = saveManager.GetData().clockCompleted[levelObj.ID] != true && earnedClock;

                if (!newEarnedCompleted && !newEarnedClock && !newEarnedMaxMoves)
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
            SceneManager.LoadScene("Level-" + (levelObj.ID + 1).ToString());
        }

        public void OnRetryLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}