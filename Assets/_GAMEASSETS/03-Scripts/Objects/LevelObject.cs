using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    [CreateAssetMenu(fileName = "New Level", menuName = "LittleSubmarine/Level", order = 51)]
    public class LevelObject : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private int worldID;
        [SerializeField] private int maxMovesForStar;
        [SerializeField] private int maxSecondsForClock;
        [SerializeField] private String scene;

        public int ID => id;
        public int WorldID => worldID;
        public int MaxMovesForStar => maxMovesForStar;
        public int MaxSecondsForClock => maxSecondsForClock;
        public String Scene => scene;
    }
}