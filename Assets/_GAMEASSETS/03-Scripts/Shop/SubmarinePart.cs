using UnityEngine;

namespace LittleSubmarine2
{
    public class SubmarinePart : MonoBehaviour
    {
        [SerializeField] private SubmarinePartType type;
        [SerializeField] private int id;
        [SerializeField] private int cost;
        [SerializeField] private Sprite spriteImage;
        [SerializeField] private string description;

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