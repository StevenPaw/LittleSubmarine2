using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    [CreateAssetMenu(fileName = "New Level", menuName = "LittleSubmarine/Level", order = 51)]
    public class LevelObject : ScriptableObject, IComparable<LevelObject>
    {
        [SerializeField] private int id;
        [SerializeField] private int worldID;
        [SerializeField] private int maxMovesForStar;
        [SerializeField] private int maxSecondsForClock;
        [SerializeField] private String scene;

        [Header("Boss Specific")] 
        [SerializeField] private bool isBossLevel;
        [SerializeField] private string bossTitle;

        public int ID => id;
        public int WorldID => worldID;
        public int MaxMovesForStar => maxMovesForStar;
        public int MaxSecondsForClock => maxSecondsForClock;
        public String Scene => scene;
        public bool IsBossLevel => isBossLevel;
        public string BossTitle => bossTitle;

        public int CompareTo(LevelObject other)
        {
            // A null value means that this object is greater.
            if (other == null)
            {
                return 1;
            }
            return ID.CompareTo(other.ID);
        }
    }
}