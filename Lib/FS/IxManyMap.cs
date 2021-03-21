using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS {
    public class IxManyMap<K, V> : IEnumerable<KeyValuePair<K, V>>
        where K: IComparable
    {
        private ulong _tick = 0;
        private SortedDictionary<K, List<V>> _dictionary = new();
        
        public IxManyMap() { } 

        public void Add(K k, V v) {
            _tick++;

            if (!_dictionary.TryGetValue(k, out List<V> vs)) {
                vs.Add(v);
            }
            else {
                vs = new();
                vs.Add(v);
                _dictionary[k] = vs;
            }
        }

        public void Insert(K k, int i, V v) {
            _tick++;

            if (!_dictionary.TryGetValue(k, out List<V> vs)) {
                vs.Insert(i, v);
            }
            else {
                vs = new();
                vs.Insert(i, v);
                _dictionary[k] = vs;
            }
        }

        public bool ContainsIndex(K k, int iv) {
            if (_dictionary.TryGetValue(k, out List<V> oldVs)) {
                return 0 <= iv && iv < oldVs.Count;
            }
            return false;
        }

        public bool ContainsKey(K k) => _dictionary.ContainsKey(k);

        public bool RemoveIndex(K k, int iv) => RemoveIndex(k, iv, out V v);
        public bool RemoveIndex(K k, int iv, out V v) {
            _tick++;
            v = default(V);
            if (!ContainsIndex(k, iv)) {
                return false;
            }

            var vs = _dictionary[k];
            var element = vs[iv];
            v = element;
            vs.RemoveAt(iv);
            if (!vs.Any()) {
                _dictionary.Remove(k);
            }

            return true;
        }

        public List<V> PopKey(K k) {
            _tick++;
            if (_dictionary.Remove(k, out List<V> vs)) {
                return vs;
            }
            return new();
        }

        public IxMany<V> this[K k] {
            get => new ViewIxMany(this, k);
        }

        public V this[K k, int i] {
            get => _dictionary[k][i];
            set => _dictionary[k][i] = value;
        }

        private IEnumerable<V> AllVsFromK(K k) {
            var _old = _tick;
            if (_dictionary.TryGetValue(k, out List<V> vs)) {
                foreach (var v in vs) {
                    yield return v;
                    if (_tick != _old) {
                        throw new InvalidOperationException("cannot iterate: underlying object changed");
                    }
                }
            }
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() {
            var _old = _tick;

            foreach (var kvs in _dictionary) {
                var k = kvs.Key;
                var vs = kvs.Value;
                foreach (var v in vs) {
                    yield return new(k, v);
                    if (_tick != _old) {
                        throw new InvalidOperationException("cannot iterate: underlying object changed");
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class ViewIxMany : IxMany<V> {
            private IxManyMap<K, V> _this;
            private K _k;

            public ViewIxMany(IxManyMap<K, V> impl, K k) {
                _this = impl;
                _k = k;
            }

            public override void Add(V v) => _this.Add(_k, v);
            public override void Insert(int i, V v) => _this.Insert(_k, i, v);
            public override bool Contains(int iv) => _this.ContainsIndex(_k, iv);
            public override bool Remove(int iv) => _this.RemoveIndex(_k, iv);
            public override bool Remove(int iv, out V v) => _this.RemoveIndex(_k, iv, out v);

            public override V this[int i] {
                get => _this[_k, i];
                set => _this[_k, i] = value;
            }
            public override bool TryGetValue(int i, out V v) {
                if (Contains(i)) {
                    v = this[i];
                    return true;
                }
                v = default(V);
                return false;
            }

            public override IEnumerator<V> GetEnumerator() {
                foreach (var b in _this.AllVsFromK(_k)) {
                    yield return b;
                }
            }
        }
    }

}
