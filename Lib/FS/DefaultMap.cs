using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS {
    public class DefaultMap<K, V> 
        where K: IComparable
        where V: struct, IComparable
    {
        private V _default;
        private Map<K, V> _map = new();
        
        public DefaultMap(V @default) {
            _default = @default;
        }

        // We don't require V: IComparable, meaning we have no idea if V is new
        // so: void
        public void Add(K k, V v) {  
            if (_default.Equals(v)) {
                _map.Remove(k);
            } else {
                _map[k] = v;
            }
        }

        public V this[K k] {
            get {
                if (_map.TryGetValue(k, out V v)) {
                    return v;
                }
                return _default;
            }
            set {
                Add(k, value);
            }
        }
    }
}
