using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleSubmarine2
{
    [Serializable]
    public class PlayerData
    {
        //All information that need to be saved
        public string dateTimeOfSave;
        public bool[] levelCompleted = new bool[200];
        
        //Shopdata
        public int coins;
        public bool[] boughtPeriscopes = new bool[200];
        public bool[] boughtBodies = new bool[200];
        public int selectedPeriscope;
        public int selectedBody;
    }
}