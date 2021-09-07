using System.Collections;
using LittleSubmarine2;
using UnityEngine;

namespace LittleSubmarine2
{
    [DisallowMultipleComponent]
    public class ActiveSaveData : MonoBehaviour
    {
        [Header("There should only be one of this script in all times!")]
        [SerializeField] private PlayerData playerData;

        [SerializeField] private int amountOfLevels;
        [SerializeField] private int amountOfWorlds;

        private SaveManager saveManager;
        
        private void Start()
        {
            //Making sure there is only one class of ActiveSaveData available at a time:
            if (GameObject.FindGameObjectWithTag(GameTags.ACTIVESAVEDATA) != this.gameObject)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }

            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();
            InitializeLevels();
            saveManager.LoadGame();
        }

        /// <summary>
        /// Set the Data of the currently active Savefile
        /// </summary>
        /// <param name="playerDataIn"></param>
        public void SetData(PlayerData playerDataIn)
        {
            playerData = playerDataIn;
        }

        /// <summary>
        /// Get the data of the current active SaveData
        /// </summary>
        /// <returns>(PlayerData) active SaveData-Variables</returns>
        public PlayerData GetData()
        {
            if (playerData != null)
            {
                return playerData;
            }

            //If there is no SaveData stored yet (for example in a new Game) use a new PlayerData-Class
            playerData = new PlayerData();
            InitializeLevels();
            return playerData;
        }

        private void InitializeLevels()
        {
            Debug.Log("Creating Level Files");
            for (int i = 0; i < amountOfWorlds; i++)
            {
                bool levelCompleted = false;
                for (int j = 1; j < amountOfLevels; j++)
                {
                    playerData.levelCompleted.Add(levelCompleted);
                }
            }
            
        }

        public void AddLevel(int worldIn, int levelIn)
        {
            playerData.levelCompleted[(worldIn * 9) + levelIn] = true;
        }
    }
}