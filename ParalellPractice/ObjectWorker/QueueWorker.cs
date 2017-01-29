using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectWorker {
    class QueueWorker :IWorker {
        Queue<object> fifoQueue;
        Thread t;
        private object locker;
        private EventWaitHandle jobReady = new AutoResetEvent(false);


        public QueueWorker() {
            fifoQueue = new Queue<object>(20);
            locker = new object();

            t = new Thread(Run);
            t.IsBackground = true;
            t.Start();

        }
        public void ProcessJobAsync(object job) {
            new Thread(() => {
                lock (locker) {

                    fifoQueue.Enqueue(job);
                }
                jobReady.Set();
            }).Start();



        }

        private void Run() {
            while (true) {

                object job = null;
                lock (locker) {
                    if (fifoQueue.Count > 0)
                        job = fifoQueue.Dequeue();
                }
                if (job != null)
                    Console.WriteLine(job);
                else
                    jobReady.WaitOne();

            }
        }
    }
}
