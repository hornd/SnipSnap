using System;
using System.Collections.Generic;

namespace SnipSnap
{
    public static class ThreadMsgQueue<T>
    {
        private static Queue<T> messages = new Queue<T>();

        public static void Enqueue(T enq)
        {
            lock (messages)
            {
                Console.WriteLine("Enq: " + enq.ToString());
                messages.Enqueue(enq);
            }
        }

        public static T Dequeue()
        {
            lock (messages)
            {
                if (messages.Count == 0) 
                   // TODO
                   // return default(T);  

                T ret = messages.Dequeue();
                Console.WriteLine("Deq: " + ret);
                //return messages.Dequeue();
                return ret;
            }
        }
    }
}