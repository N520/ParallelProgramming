using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectWorker {
    class ConcurrentQueueWorker : IWorker {
        ConcurrentQueue<object> fifoQueue;
        Thread t;
        private object locker;
        private EventWaitHandle jobReady = new AutoResetEvent(false);


        public ConcurrentQueueWorker() {
            fifoQueue = new ConcurrentQueue<object>();
            locker = new object();

            t = new Thread(Run);
            t.IsBackground = true;
            t.Start();

        }
        public void ProcessJobAsync(object job) {
            new Thread(() => {
                fifoQueue.Enqueue(job);
                jobReady.Set();
            }).Start(); ;



        }

        private void Run() {
            while (true) {

                object job = null;

                if (fifoQueue.TryDequeue(out job))
                    Console.WriteLine(job);
                else
                    jobReady.WaitOne();

            }
        }
    }
}
