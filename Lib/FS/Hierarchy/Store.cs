using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS.Hierarchy {
    public class Store<Phantom>: AnyStore where Phantom: IDuration {
        private SortedDictionary<string, IPrimitive> _entries = new();
        private SortedDictionary<string, Action> _onLoad = new();

        public Store() { }

        public Node<Phantom> Root(string name) => new Node<Phantom>(this, name);

        public void Bind(string path, IPrimitive primitive, Action onLoad) {
            if (_entries.ContainsKey(path)) {
                throw new InvalidOperationException($"can't bind a primitive to {path} twice");
            }
            // TODO: Check for duplicates
            _entries[path] = primitive;
            if (onLoad != null) {
                _onLoad[path] = onLoad;
            }
        }

        // TODO: Use a SortedDictionary<string, Union<string, SortedDictionary<...>> instead?
        // Basically a tree, to accomodate nested stores. Will require assigning a new type to Dump.
        public SortedDictionary<string, string> Save() {
            var result = new SortedDictionary<string, string>();
            foreach (var e in _entries) {
                result[e.Key] = e.Value.Dump();
            }
            return result;
        }

        public void Load(SortedDictionary<string, string> sd) {
            foreach (var e in _entries) {
                if (sd.TryGetValue(e.Key, out string s)) {
                    e.Value.Load(s);
                }
            }
            foreach (var ol in _onLoad) {
                ol.Value();
            }
        }
    }
}
