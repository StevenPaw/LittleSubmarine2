using System;
using System.Collections;
using System.Collections.Generic;
using LittleSubmarine2;
using UnityEngine;

namespace LittleSubmarine2
{
    public class GameManager : MonoBehaviour
    {
        private SaveManager saveManager;
        
        private void Start()
        {
            //Make sure there is only one instance of the GameManager
            if (GameObject.FindGameObjectWithTag(GameTags.GAMEMANAGER) == this.gameObject)
            {
                DontDestroyOnLoad(this.gameObject);
                Debug.Log("GameManager loaded");
            }
            else
            {
                Destroy(this.gameObject);
                Debug.Log("GameManager destroyed");
            }

            
        }
    }
}