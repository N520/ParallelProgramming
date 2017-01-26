using System;
using System.Collections.Concurrent;
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
        private static EventWaitHandle ready = new AutoResetEvent(false);
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

            //Console.WriteLine("### Tasks sequential ###");
            //foreach (var item in taskData) {
            //    item.Start();
            //}
            //Console.WriteLine("MainThread is sleeping");
            //Thread.Sleep(10000);
            //Console.WriteLine("MainThread continues");


            Console.WriteLine("### consumer Producer with BlockingCollection ###");
            BlockingCollection<Product> dataList = new BlockingCollection<Product>(10);
            Task producer = Task.Run(() => {
                for (int i = 0; i < 20; i++) {
                    var product = Produce(i, "Car " + i);
                    if (!dataList.TryAdd(product))
                        Console.WriteLine($"Couldnt add {product}");
                }
                dataList.CompleteAdding();
            });
            Thread.Sleep(2000);
            Task consumer = Task.Run(() => {
                while (!dataList.IsCompleted) {
                    Product consumee;
                    if (dataList.TryTake(out consumee)) {
                        Consume(consumee);
                    } else {
                        Console.WriteLine("list is empty");
                    }
                    
                }
                ready.Set();
            });
            ready.WaitOne();
            Console.WriteLine("Done!!");
            
        }

        private static Product Produce(int id, string name) {
            Console.WriteLine($"Producing {id}: {name}");
            Thread.Sleep(1000);
            return new Product(id, name);
        }

        private static void Consume(Product product) {
            Console.WriteLine($"Consuming {product.Id}: {product.Name}");
            Thread.Sleep(3000);
        }


    }
}
