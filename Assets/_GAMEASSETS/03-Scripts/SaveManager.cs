using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LittleSubmarine2
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private string fileName = "gamedata";
        [SerializeField] private string fileEnding = ".gamesave"; //the fileEnding used for the saveFiles

        private string saveFilePath; //The path where the saveFiles are stored
        private PlayerData playerData;
        private static SaveManager instance;

        public static SaveManager Instance => instance;

        private void Start()
        {
            if (GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER) == gameObject)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
                Debug.Log("SaveManager set!");
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("SaveManager destroyed");
            }

            saveFilePath = Application.persistentDataPath;
            
            LoadGame();
        }

        //Saving Game etc.

        public void SaveGame()
        {
            playerData.dateTimeOfSave = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
            WriteDataToJson();
        }

        public void LoadGame()
        {
            playerData = ReadDataFromJson();
        }

        public void ClearSave()
        {
            Debug.Log("Deleting Save " + saveFilePath + fileName + fileEnding);
            File.Delete(saveFilePath + "/" + fileName + fileEnding);
            playerData = new PlayerData();
            //LoadGame();
        }
        
        public void WriteDataToJson() {
            string dataString;
            string jsonFilePath = DataPath();
            CheckFileExistance(jsonFilePath);
 
            dataString = JsonUtility.ToJson(playerData);
            File.WriteAllText(jsonFilePath, dataString);
        }
        
        public PlayerData ReadDataFromJson() {
            string dataString;
            string jsonFilePath = DataPath();
            CheckFileExistance(jsonFilePath, true);
 
            dataString = File.ReadAllText(jsonFilePath);
            playerData = JsonUtility.FromJson<PlayerData>(dataString);
            return playerData;
        }
        
        public void CheckFileExistance(string filePath, bool isReading = false) {
            if (!File.Exists(filePath)){
                File.Create(filePath).Close();
                if (isReading) {
                    Debug.Log("PlayerData is empty! " + filePath);
                    playerData = new PlayerData();
                    playerData.levelCompleted = new bool[255];
                    playerData.maxMovesCompleted = new bool[255];
                    playerData.clockCompleted = new bool[255];
                    string dataString = JsonUtility.ToJson(playerData);
                    File.WriteAllText(filePath, dataString);
                }
            }
        }
        
        public string DataPath() {
            if (Directory.Exists(Application.persistentDataPath)) {
                return Path.Combine(Application.persistentDataPath, fileName + fileEnding);
            }
            return Path.Combine(Application.streamingAssetsPath, fileName + fileEnding);
        }

        public PlayerData GetData()
        {
            return playerData;
        }

        public void AddCompletedLevel(int level, bool maxMove, bool clock)
        {
            playerData.levelCompleted[level] = true;
            playerData.maxMovesCompleted[level] = maxMove;
            playerData.clockCompleted[level] = clock;
            Debug.Log("Adding Clock to Save: " + clock);
            SaveGame();
        }

        /// <summary>
        /// Increase or Decrease the amount of coins the player has
        /// </summary>
        /// <param name="coinsIn"></param>
        public void ChangeCoins(int coinsIn)
        {
            playerData.coins += coinsIn;
        }

        /// <summary>
        /// Get the amount of money the player has
        /// </summary>
        /// <returns></returns>
        public int GetCoins()
        {
            return playerData.coins;
        }
        
        public void AddCoins(int addedCoins)
        {
            playerData.coins += addedCoins;
        }

        public bool[] GetBoughtPeriscopes()
        {
            return playerData.boughtPeriscopes;
        }
        
        public bool[] GetBoughtBodies()
        {
            return playerData.boughtBodies;
        }

        public void BuyBody(int idIn, int costIn)
        {
            playerData.coins -= costIn;
            playerData.boughtBodies[idIn] = true;
            SaveGame();
        }
        
        public void BuyPeriscope(int idIn, int costIn)
        {
            playerData.coins -= costIn;
            playerData.boughtPeriscopes[idIn] = true;
            SaveGame();
        }

        public void SelectSubmarine(int periscopeID, int bodyID)
        {
            playerData.selectedBody = bodyID;
            playerData.selectedPeriscope = periscopeID;
            SaveGame();
        }
    }
}