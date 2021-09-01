using UnityEngine;

namespace LittleSubmarine2
{
    public interface IPushable
    {
        public bool Push(Vector2 moveDirectionIn, float moveSpeedIn);
        public void UndoPush();
        public PushableTypes GetPushableType();
    }
}