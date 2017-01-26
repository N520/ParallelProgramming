using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParalellLoops {
    class Program {
        private static Stopwatch sw;
        private static FakeDataProvider dataProvider;
        private static WaitHandle ready = new AutoResetEvent(false);
        static void Main(string[] args) {
            dataProvider = new FakeDataProvider();
            IEnumerable<int> testData = Enumerable.Range(0, 10000);
            object locker = new object();

            Console.WriteLine("### sequencial ###");
            sw = Stopwatch.StartNew();
            long sum = testData.Sum();
            sw.Stop();
            Console.WriteLine(sum);
            Console.WriteLine(sw.Elapsed);

            Console.WriteLine("### paralell ###");
            sw = Stopwatch.StartNew();
            sum = testData.AsParallel().Sum();
            sw.Stop();
            Console.WriteLine(sum);
            Console.WriteLine(sw.Elapsed);

            sum = 0;

            Console.WriteLine("### paralell divide ###");
            sw = Stopwatch.StartNew();
            Parallel.ForEach(testData, () => 0, (x, loopState, partialResult) => {
                return x + partialResult;
            }, (partialResult) => {
                lock (locker) {
                    sum += partialResult;
                }
            });

            sw.Stop();
            Console.WriteLine(sum);
            Console.WriteLine(sw.Elapsed);

            IList<Task> taskData = new List<Task>();

            for (int i = 0; i < 20; i++) {
                taskData.Add(new Task(() => {
                    dataProvider.LoadFakeData();
                    dataProvider.ProcessFakeData();
                    dataProvider.SaveFakeData();
                }));
            }

            Console.WriteLine("### Tasks sequential ###");
            foreach (var item in taskData) {
                item.Start();
            }
            Console.WriteLine("MainThread is sleeping");
            Thread.Sleep(10000);
            Console.WriteLine("MainThread continues");

        }


    }
}
