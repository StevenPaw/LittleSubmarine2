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

        private void Start()
        {
            if (GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER) == this.gameObject)
            {
                DontDestroyOnLoad(this.gameObject);
                Debug.Log("SaveManager loaded");
            }
            else
            {
                Destroy(this.gameObject);
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
            File.Delete(saveFilePath + fileName + fileEnding);
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
            Debug.Log("Checking File! " + filePath);
            if (!File.Exists(filePath)){
                File.Create(filePath).Close();
                if (isReading) {
                    Debug.Log("PlayerData is empty! " + filePath);
                    playerData = new PlayerData();
                    playerData.levelCompleted = new bool[200];
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
            Debug.Log("PlayerData: " + playerData.levelCompleted);
            return playerData;
        }

        public void AddCompletedLevel(int world, int level)
        {
            playerData.levelCompleted[(world * 9) + level] = true;
            SaveGame();
        }
    }
}