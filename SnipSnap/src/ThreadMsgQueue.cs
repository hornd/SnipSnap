using System;
using System.Collections.Concurrent;

namespace SnipSnap
{
    public static class ThreadMsgQueue<T>
    {
        private static BlockingCollection<T> messages = new BlockingCollection<T>();

        public static void Enqueue(T enq)
        {
            messages.Add(enq);
        }

        public static T Dequeue()
        {
            return messages.Take();            
        }
    }
}
