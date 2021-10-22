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
        [SerializeField] private Image[] starImage;
        [SerializeField] private Sprite starSpriteFilled;
        [SerializeField] private Sprite starSpriteEmpty;
        [SerializeField] private int maxMovesForStar2;
        [SerializeField] private int maxMovesForStar3;
        
        [Header("Clock")]
        [SerializeField] private Image clockImage;
        [SerializeField] private Sprite clockSpriteFilled;
        [SerializeField] private Sprite clockSpriteEmpty;
        [SerializeField] private int maxSecondsForClock;

        private int earnedStars;
        private bool earnedClock;
        private int extraCoins = 0;

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
                starImage[0].DOCrossfadeImage(starSpriteFilled, 0.5f);
                
                if (usedMoves <= maxMovesForStar2)
                {
                    Debug.Log("UsedMoves: " + usedMoves + " for max (2): " + maxMovesForStar2);
                    starImage[1].DOCrossfadeImage(starSpriteFilled, 1.0f);
                    earnedStars += 1;
                }
                if (usedMoves <= maxMovesForStar3)
                {
                    starImage[2].DOCrossfadeImage(starSpriteFilled, 1.5f);
                    earnedStars += 1;
                }

                if (usedTime.TotalSeconds < maxSecondsForClock)
                {
                    clockImage.DOCrossfadeImage(clockSpriteFilled, 2.0f);
                    earnedClock = true;
                    extraCoins += coinsPerStar;
                }

                int newEarnedStars = earnedStars - saveManager.GetData().levelCompleted[(world * 9) + level];
                bool newEarnedClock = saveManager.GetData().clockCompleted[(world * 9) + level] != true && earnedClock;

                if (newEarnedStars <= 0 && !newEarnedClock)
                {
                    addedCoinsGO.SetActive(false);
                }
                else
                {
                    addedCoinsGO.SetActive(true);
                    coinsAmountText.text = "+ " + coinsPerStar * newEarnedStars;
                    saveManager.AddCoins(coinsPerStar * newEarnedStars + extraCoins);
                    saveManager.AddCompletedLevel(world, level, earnedStars, earnedClock);
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

        public void OnRetryLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}