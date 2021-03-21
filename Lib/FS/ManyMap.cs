using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS {
    public class ManyMap<K, V> 
        where K: IComparable
        where V: IComparable
    {
        private ulong _tick = 0;
        private SortedDictionary<K, SortedSet<V>> _dictionary = new();
        
        public ManyMap() { } 

        public bool Add(K k, V v) {
            _tick++;

            if (Contains(k, v)) {
                return false;
            }

            if (!_dictionary.TryGetValue(k, out SortedSet<V> vs)) {
                vs.Add(v);
            }
            else {
                vs = new SortedSet<V>();
                vs.Add(v);
                _dictionary[k] = vs;
            }

            return true;
        }

        public bool Contains(KeyValuePair<K, V> kv) => Contains(kv.Key, kv.Value);
        public bool Contains(K k, V v) {
            if (_dictionary.TryGetValue(k, out SortedSet<V> oldVs)) {
                return oldVs.Contains(v);
            }
            return false;
        }

        public bool ContainsKey(K k) => _dictionary.ContainsKey(k);

        public bool Remove(KeyValuePair<K, V> kv) => Remove(kv.Key, kv.Value);
        public bool Remove(K k, V v) {
            _tick++;
            if (!Contains(k, v)) {
                return false;
            }

            var vs = _dictionary[k];
            vs.Remove(v);
            if (!vs.Any()) {
                _dictionary.Remove(k);
            }

            return true;
        }

        public SortedSet<V> PopKey(K k) {
            _tick++;
            if (_dictionary.Remove(k, out SortedSet<V> vs)) {
                return vs;
            }
            return new();
        }

        public Many<V> this[K k] {
            get => new ViewMany(this, k);
        }

        private IEnumerable<V> AllVsFromK(K k) {
            var _old = _tick;
            if (_dictionary.TryGetValue(k, out SortedSet<V> vs)) {
                foreach (var v in vs) {
                    yield return v;
                    if (_tick != _old) {
                        throw new InvalidOperationException("cannot iterate: underlying object changed");
                    }
                }
            }
        }

        public class ViewMany : Many<V> {
            private ManyMap<K, V> _this;
            private K _k;

            public ViewMany(ManyMap<K, V> impl, K k) {
                _this = impl;
                _k = k;
            }

            public override bool Add(V v) => _this.Add(_k, v);
            public override bool Contains(V v) => _this.Contains(_k, v);
            public override bool Remove(V v) => _this.Remove(_k, v);

            public override IEnumerator<V> GetEnumerator() {
                foreach (var b in _this.AllVsFromK(_k)) {
                    yield return b;
                }
            }
        }
    }

}
