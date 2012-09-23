using System;
using System.Collections.Concurrent;

namespace SnipSnap
{
    public static class ThreadMsgQueue<T>
    {
        private static BlockingCollection<T> messages = new BlockingCollection<T>();

        public static void Enqueue(T enq)
        {
            Console.WriteLine("Enq");
            messages.Add(enq);
        }

        public static T Dequeue()
        {
            Console.WriteLine("Deq");
            return messages.Take();            
        }
    }
}

// 0xa2 or 0xa3 - ctrl
// 0x2d - ins