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
        public int[] levelCompleted = new int[256];
        public bool[] clockCompleted = new bool[256];
        
        //Shopdata
        public int coins;
        public bool[] boughtPeriscopes = new bool[256];
        public bool[] boughtBodies = new bool[256];
        public int selectedPeriscope;
        public int selectedBody;
    }
}