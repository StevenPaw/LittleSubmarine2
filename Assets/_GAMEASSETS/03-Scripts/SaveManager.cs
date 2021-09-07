using System;
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
        private ActiveSaveData activeSaveData;
        
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
            
            activeSaveData = GameObject.FindGameObjectWithTag(GameTags.ACTIVESAVEDATA).GetComponent<ActiveSaveData>();
            
            LoadGame();
        }

        //Saving Game etc.

        public void SaveGame()
        {
            SaveFileToJson();
        }

        public void LoadGame()
        {
            playerData = LoadFromJson();
            activeSaveData.SetData(playerData);
        }

        public void ClearSave()
        {
            activeSaveData.ClearData();
            SaveGame();
        }
        
        private PlayerData LoadFromJson()
        {
            PlayerData playerDataOut = new PlayerData();
            if (File.Exists(saveFilePath + "/" + fileName + fileEnding))
            {
                string loadedPlayerData = File.ReadAllText(saveFilePath + "/" + fileName + fileEnding);
                playerDataOut = JsonUtility.FromJson<PlayerData>(loadedPlayerData);
            }
            else
            {
                Debug.Log("No PlayerData SaveFile found");
            }

            return playerDataOut;
        }
        
        private void SaveFileToJson()
        {
            playerData = activeSaveData.GetData();
            playerData.dateTimeOfSave = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
            string playerDataToSave = JsonUtility.ToJson(playerData);
            File.WriteAllText(saveFilePath + "/" + fileName + fileEnding, playerDataToSave);
        }
    }
}