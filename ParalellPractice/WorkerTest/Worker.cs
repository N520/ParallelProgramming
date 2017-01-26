using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerTest {
    class Worker {
        Thread t;
        CancellationTokenSource cancelSource;
        CancellationToken cancel;
        int initialValue;
        public Worker(int initialvalue,  CancellationToken token) {
            this.initialValue = initialvalue;
            cancel = token;
        }

        public Worker(int initialValue) {
            this.initialValue = initialValue;
            cancelSource = new CancellationTokenSource();
            cancel = cancelSource.Token;
        }


        public void Run() {
            t = new Thread(() => {
                Console.WriteLine("Worker is working really hard!");
                var value = initialValue;
                while (true) {
                    if (cancel.IsCancellationRequested)
                        return;
                    Console.WriteLine(value++);
                    Thread.Sleep(1000);
                }
            });
            t.Start();
        }

        public void Stop() {
            if (cancelSource == null)
                return;
            cancelSource.Cancel();
        }
    }
}
