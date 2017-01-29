using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectWorker {
    class Program {
        static void Main(string[] args) {
            IWorker worker = new QueueWorker();
            worker.ProcessJobAsync("Yay");
            worker.ProcessJobAsync("hello");
            worker.ProcessJobAsync("world");
            worker.ProcessJobAsync("this");
            worker.ProcessJobAsync("is");
            worker.ProcessJobAsync("nothing ");
            worker.ProcessJobAsync("but");

            Console.WriteLine("Meanwhile this one here is free to do whatever thefuck helikes");


            var b = new int[] { 1, 3, -5, 5, 2, 1, 6, 7, 8, 9, 1 };
            var a = new int[] { 1, 3, -5, 5, 2, 1, 6, 7, 8, 9, 1 };
            Console.WriteLine(DotProduct(new int[] { 1, 3, -5, 5, 2, 1, 6, 7, 8, 9, 1 }, new int[] { 1, 3, -5, 5, 2, 1, 6, 7, 8, 9, 1 }));

            Console.WriteLine(a.AsParallel().Zip(b.AsParallel(), (x, y) => x * y).Sum());

        }

        private static int DotProduct(int[] vec1, int[] vec2) {
            if (vec1 == null)
                return 0;

            if (vec2 == null)
                return 0;

            if (vec1.Length != vec2.Length)
                return 0;

            int tVal = 0;
            //Parallel.For(0, vec1.Length, (x) => {
            //    var temp = vec1[x] * vec2[x];

            //    Interlocked.Add(ref tVal, temp);
            //});

            for (int i = 0; i < vec1.Length; i++) {
                tVal += vec1[i] * vec2[i];
            }

            return tVal;
        }
    }
}
