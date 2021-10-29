using UnityEngine;

namespace LittleSubmarine2
{
    public interface IPushable : IMovable
    {
        public bool Push(Vector2 direction);
        public void UndoPush();
        public PushableTypes GetPushableType();
    }
}