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
        [SerializeField] private string nextLevel;
        [SerializeField] private int world;
        [SerializeField] private int level;

        [SerializeField] private GameObject addedCoinsGO;
        [SerializeField] private int coinsPerStar;
        [SerializeField] private TMP_Text coinsAmountText;

        [SerializeField] private TMP_Text movesText;
        [SerializeField] private TMP_Text timeText;

        [Header("Stars")] 
        [SerializeField] private Image star1Filled;
        [SerializeField] private Image star2Filled;
        [SerializeField] private Image star3Filled;
        [SerializeField] private int maxMovesForStar2;
        [SerializeField] private int maxMovesForStar3;

        private int earnedStars;

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
                
                earnedStars = 1;
                star1Filled.DOFade(1f, 0.5f);
                
                if (usedMoves <= maxMovesForStar2)
                {
                    Debug.Log("UsedMoves: " + usedMoves + " for max (2): " + maxMovesForStar2);
                    star2Filled.DOFade(1f, 1f);
                    earnedStars += 1;
                }
                if (usedMoves <= maxMovesForStar3)
                {
                    star3Filled.DOFade(1f, 1.5f);
                    earnedStars += 1;
                }
                
                Debug.Log("Earned Stars: " + earnedStars);
                
                int newEarnedStars = earnedStars - saveManager.GetData().levelCompleted[(world * 9) + level];
                
                Debug.Log("new Earned Stars: " + newEarnedStars);
                
                if (newEarnedStars == 0)
                {
                    addedCoinsGO.SetActive(false);
                }
                else
                {
                    addedCoinsGO.SetActive(true);
                    coinsAmountText.text = "+ " + coinsPerStar * newEarnedStars;
                    saveManager.AddCoins(coinsPerStar * newEarnedStars);
                    saveManager.AddCompletedLevel(world, level, earnedStars);
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
            SceneManager.LoadScene(nextLevel);
        }
    }
}