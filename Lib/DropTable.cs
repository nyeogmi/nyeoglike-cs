using Nyeoglike.Lib.FS;
using Nyeoglike.Lib.FS.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib {
    public class DropTable<T> : IEnumerable<KeyValuePair<ID<T>, T>> {
        // TODO: Provide deleted-marking, but not actual deletion
        private Map<ID<T>, T> _ts;
        private int _next;

        public DropTable(AnyNode node) {
            _ts = new(node);
            _next = 1;
        }

        public ID<T> Add(T t) {
            var id = new ID<T>(_next++);
            _ts[id] = t;

            return id;
        }

        public ID<T> Add(Func<ID<T>, T> f) {
            var id = new ID<T>(_next++);
            _ts[id] = f(id);

            return id;
        }

        public bool Remove(ID<T> id) => _ts.Remove(CheckID(id));
        public bool Remove(ID<T> id, out T t) => _ts.Remove(CheckID(id), out t);

        public bool ContainsKey(ID<T> id) => _ts.ContainsKey(id);

        public bool TryGetValue(ID<T> id, out T t) =>
            _ts.TryGetValue(CheckID(id), out t);

        public T GetValueOrDefault(ID<T> id, T @default) {
            if (_ts.TryGetValue(CheckID(id), out T t)) {
                return t;
            }
            return @default;
        }

        public T ForceGetValue(ID<T> id) {
            if (_ts.TryGetValue(CheckID(id), out T t)) {
                return t;
            }
            throw new ArgumentException($"can't get deleted thing: {id}");
        }

        public T this[ID<T> id] { 
            get {
                id = CheckID(id);
                return _ts[id];
            }
            set {
                id = CheckID(id);
                _ts[id] = value;
            }
        }

        private ID<T> CheckID(ID<T> id) {
            if (!id.Initialized) {
                throw new ArgumentException($"ID must be initialized: {id}");
            }

            if (!_ts.ContainsKey(id)) {
                throw new ArgumentException($"must already contain ID: {id}");
            }
            return id;
        }

        public IEnumerator<KeyValuePair<ID<T>, T>> GetEnumerator() {
            // TODO: Error when changes are made out from under me
            foreach (var kv in _ts) {
                yield return kv;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            GetEnumerator();

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
