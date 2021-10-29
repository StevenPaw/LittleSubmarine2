using UnityEngine;

namespace LittleSubmarine2
{
    public interface IMovable
    {
        public Transform MovePoint { get; set; }
        public Collider2D SelfCollider { get; set; }
        public HistoryManager History { get; set; }
        public Animator Anim { get; set; }
    }
}