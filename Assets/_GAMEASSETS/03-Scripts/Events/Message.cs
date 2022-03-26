using System;

namespace LittleSubmarine2.Events
{
    public class Message<T>
    {
        public static Message<T> Instance = new Message<T>();
        
        private Action<T> delegateAction = delegate {  };

        private void InternalAdd(Action<T> callback)
        {
            delegateAction += callback;
        }

        public static void Add(Action<T> callback)
        {
            Instance.InternalAdd(callback);
        }

        private void InternalRemove(Action<T> callback)
        {
            delegateAction -= callback;
        }

        public static void Remove(Action<T> callback)
        {
            Instance.InternalRemove(callback);
        }

        private void InternalRaise(T genericEvent)
        {
            delegateAction(genericEvent);
        }

        public static void Raise(T genericEvent)
        {
            Instance.InternalRaise(genericEvent);
        }
    }

    public static class Message
    {
        public static void Raise<T>(T genericEvent)
        {
            Message<T>.Raise(genericEvent);
        }
    }
}