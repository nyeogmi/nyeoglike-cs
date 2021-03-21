using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS {
    public class NullMap<K, V> : IEnumerable<KeyValuePair<K, V>>
        where K: IComparable
        where V: struct
    {
        private Map<K, V> _map = new();
        
        public NullMap() { }

        // We don't require V: IComparable, meaning we have no idea if V is new
        // so: void
        public void Add(K k, V v) {  
            _map[k] = v;
        }

        public bool ContainsKey(K k) => _map.ContainsKey(k);

        public bool Remove(K k) => Remove(k, out V v);
        public bool Remove(K k, out V v) {
            return _map.Remove(k, out v);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => _map.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public V? this[K k] {
            get {
                if (_map.TryGetValue(k, out V v)) {
                    return v;
                }
                return null;
            }
            set {
                if (value.HasValue) {
                    Add(k, value.Value);
                }
                else {
                    Remove(k);
                }
            }
        }

        public bool TryGetValue(K k, out V v) => _map.TryGetValue(k, out v);
    }

    public static class NullMapOps {
        public static bool Contains<K, V>(this NullMap<K, V> map, K k, V v) 
            where K : IComparable 
            where V : struct, IComparable  // TODO: IEquatable?
        {
            if (map.TryGetValue(k, out V oldV)) {
                return oldV.Equals(v);
            }
            return false;
        }

        public static bool Contains<K, V>(this NullMap<K, V> map, KeyValuePair<K, V> kv) 
            where K : IComparable 
            where V : struct, IComparable  // TODO: IEquatable?
        {
            return Contains(map, kv.Key, kv.Value);
        }
    }
}
