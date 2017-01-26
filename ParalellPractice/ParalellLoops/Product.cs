using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParalellLoops {
    class Product {
        public int Id { get; set; }
        public string Name { get; set; }

        public Product(int id, string name) {
            Id = id;
            Name = name;
        }
    }
}
