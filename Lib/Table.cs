using Nyeoglike.Lib.FS.Hierarchy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib {
    public class Table<T> : IEnumerable<KeyValuePair<ID<T>, T>>, IPrimitive {
        // TODO: Provide deleted-marking, but not actual deletion
        private List<T> _ts;

        public Table(AnyNode node) {
            node.Bind(this);
            _ts = new List<T>();
        }

        public ID<T> Add(T t) {
            var ix = _ts.Count;
            _ts.Add(t);

            return new ID<T>(ix + 1);
        }

        public ID<T> Add(Func<ID<T>, T> f) {
            var ix = _ts.Count;
            var id = new ID<T>(ix + 1);
            _ts.Add(f(id));

            return id;
        }

        public T this[ID<T> id] {
            get {
                var ix = CheckID(id);
                return _ts[ix];
            }
            set {
                var ix = CheckID(id);
                _ts[ix] = value;
            }
        }

        private int CheckID(ID<T> id) {
            if (!id.Initialized) {
                throw new ArgumentException($"ID must be initialized: {id}");
            }

            var ix = id.Index - 1;
            if (ix < 0) {
                throw new ArgumentException($"ID too low: {id}");
            }
            if (ix >= _ts.Count) {
                throw new ArgumentException($"ID too high: {id}");
            }
            return ix;
        }

        public IEnumerator<KeyValuePair<ID<T>, T>> GetEnumerator() {
            // TODO: Error when changes are made out from under me?
            foreach (var i in Enumerable.Range(0, _ts.Count)) {
                yield return KeyValuePair.Create(new ID<T>(i + 1), _ts[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public string Dump() {
            throw new NotImplementedException();
        }

        public void Load(string s) {
            throw new NotImplementedException();
        }

        public IEnumerable<ID<T>> Keys {
            get {
                foreach (var kv in this) { 
                    yield return kv.Key; 
                }
            }
        }

        public IEnumerable<T> Values {
            get {
                foreach (var kv in this) { 
                    yield return kv.Value; 
                }
            }
        }
    }
}
