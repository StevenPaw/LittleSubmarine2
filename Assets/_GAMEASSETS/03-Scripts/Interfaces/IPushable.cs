using UnityEngine;

namespace LittleSubmarine2
{
    public interface IPushable
    {
        public bool Push(Vector2 direction, float moveSpeedIn);
        public void UndoPush();
        public PushableTypes GetPushableType();
    }
}