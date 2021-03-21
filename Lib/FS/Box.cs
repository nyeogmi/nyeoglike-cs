using Nyeoglike.Lib.FS.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS {
    public class Box<V> : IPrimitive {
        public V X { get; set; }
        
        public Box(AnyNode node) { 
            node.Bind(this);
        }

        public void Default(V v) {
            if (X == null) {
                X = v;
            }
        }

        public string Dump() {
            throw new NotImplementedException();
        }

        public void Load(string s) {
            throw new NotImplementedException();
        }
    }
}
