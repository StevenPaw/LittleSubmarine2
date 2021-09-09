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
    }
}