using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS {
    public class Map<K, V>
        where K: IComparable
    {
        private ulong _tick = 0;
        private SortedDictionary<K, V> _dictionary = new();
        
        public Map() { }

        // We don't require V: IComparable, meaning we have no idea if V is new
        // so: void
        public void Add(K k, V v) {  
            _tick++;

            _dictionary[k] = v;
        }

        public bool ContainsKey(K k) => _dictionary.ContainsKey(k);

        public bool Remove(K k, out V v) {
            _tick++;
            return _dictionary.Remove(k, out v);
        }

        public V this[K k] {
            get => _dictionary[k];
            set => Add(k, value);
        }
        // TODO: TryGetValue etc
    }
}
