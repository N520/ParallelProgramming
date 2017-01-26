using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FeedingWorkerExample {
    class Program {
        static EventWaitHandle workerReady = new AutoResetEvent(false);
        static EventWaitHandle feederProduced = new AutoResetEvent(false);
        static EventWaitHandle finishd = new AutoResetEvent(false);
        static IList<int> workList;
        static CancellationToken token;
        static CancellationTokenSource source;
        static void Main(string[] args) {
            source = new CancellationTokenSource();
            token = source.Token;
            workList = new List<int>();
            Thread worker = new Thread(Work);
            Thread feeder = new Thread(Feed);
            worker.Start();
            feeder.Start();

            Thread.Sleep(2000);
            source.Cancel();
            finishd.WaitOne();
            Console.WriteLine("Done!");
        }

        private static void Feed() {
            Random r = new Random();
            Console.WriteLine("Feeder Created1");
            while (true) {
                if (token.IsCancellationRequested)
                    break;
                workerReady.WaitOne();
                workList.Add(r.Next());
                // if there is more than one element there is a fault
                if (workList.Count > 1)
                    throw new Exception();
                feederProduced.Set();
            }
        }

        private static void Work() {
            Console.WriteLine("Worker Created!");
            workerReady.Set();
            while (true) {
                if (token.IsCancellationRequested) {
                    foreach (var item in workList) {
                        Console.WriteLine($"Removing {item}");
                    }
                    workList.Clear();
                    finishd.Set();
                    break;
                }
                // wait for feeder to signal 
                feederProduced.WaitOne();
                var removed = workList[0];
                workList.Remove(removed);
                Console.WriteLine($"removed {removed}");
                workerReady.Set();
            }

        }
    }
}
