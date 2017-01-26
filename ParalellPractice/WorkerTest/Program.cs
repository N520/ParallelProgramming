using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerTest {
    class Program {
        static void Main(string[] args) {
            CancellationTokenSource source = new CancellationTokenSource();
            Worker workerOutsideCancel = new Worker(1, source.Token);
            Worker workerInsideCancel = new Worker(0);
            workerOutsideCancel.Run();
            workerInsideCancel.Run();
            Thread.Sleep(5000);
            source.Cancel();
            Thread.Sleep(5000);
            workerInsideCancel.Stop();
            Thread.Sleep(1000);
        }
    }
}
