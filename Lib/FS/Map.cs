using Nyeoglike.Lib.FS.Hierarchy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS {
    public class Map<K, V> : IEnumerable<KeyValuePair<K, V>>, IPrimitive
        where K: IComparable
    {
        private ulong _tick = 0;
        private SortedDictionary<K, V> _dictionary = new();
        
        public Map(AnyNode node) {
            node.Bind(this);
        }

        // We don't require V: IComparable, meaning we have no idea if V is new
        // so: void
        public void Add(K k, V v) {  
            _tick++;

            _dictionary[k] = v;
        }

        public bool ContainsKey(K k) => _dictionary.ContainsKey(k);

        public bool Remove(K k) => Remove(k, out V v);
        public bool Remove(K k, out V v) {
            _tick++;
            return _dictionary.Remove(k, out v);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() {
            var _old = _tick;

            foreach (var kv in _dictionary) {
                yield return kv;
                if (_tick != _old) {
                    throw new InvalidOperationException("cannot iterate: underlying object changed");
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public V this[K k] {
            get => _dictionary[k];
            set => Add(k, value);
        }

        public bool TryGetValue(K k, out V v) => _dictionary.TryGetValue(k, out v);

        public string Dump() {
            throw new NotImplementedException();
        }

        public void Load(string s) {
            throw new NotImplementedException();
        }
    }

    public static class MapOps {
        public static bool Contains<K, V>(this Map<K, V> map, K k, V v) 
            where K : IComparable 
            where V : IComparable   // TODO: IEquatable?
        {
            if (map.TryGetValue(k, out V oldV)) {
                return oldV.Equals(v);
            }
            return false;
        }

        public static bool Contains<K, V>(this Map<K, V> map, KeyValuePair<K, V> kv) 
            where K : IComparable 
            where V : IComparable   // TODO: IEquatable?
        {
            return Contains(map, kv.Key, kv.Value);
        }
    }
}
