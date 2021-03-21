using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS.Hierarchy {
    public class Store<Phantom>: AnyStore where Phantom: IDuration {
        private SortedDictionary<string, IPrimitive> _entries = new();

        public Store() { }

        public Node<Phantom> Root(string name) => new Node<Phantom>(this, name);

        public void Bind(string path, IPrimitive primitive) {
            if (_entries.ContainsKey(path)) {
                throw new InvalidOperationException($"can't bind a primitive to {path} twice");
            }
            // TODO: Check for duplicates
            _entries[path] = primitive;
        }
    }
}
