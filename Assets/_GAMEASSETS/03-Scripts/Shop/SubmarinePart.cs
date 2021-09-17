using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace LittleSubmarine2
{
    [Serializable]
    public class SubmarinePart
    {
        [SerializeField] private SubmarinePartType type;
        [SerializeField] private int id;
        [SerializeField] private int cost;
        [SerializeField] private Sprite spriteImage;
        [SerializeField] private string description;
        
        public SubmarinePartType Type => type;
        public int ID => id;
        public int Cost => cost;
        public Sprite SpriteImage => spriteImage;
        public string Description => description;
    }
    
    

    public enum SubmarinePartType
    {
        PERISCOPE,
        BODY
    }
}