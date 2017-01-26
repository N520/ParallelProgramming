using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParalellLoops {
    class FakeDataProvider {
    public    void LoadFakeData() {
            Thread.Sleep(2000);
            Console.WriteLine("loaded Data");
        }

        public void ProcessFakeData() {
            Thread.Sleep(1000);
            Console.WriteLine("Processed Data");
        }

        public void SaveFakeData() {
            Thread.Sleep(2000);
            Console.WriteLine("saved Data");
        }
    }
}
