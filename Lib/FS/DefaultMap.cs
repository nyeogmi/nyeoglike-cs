using Nyeoglike.Lib.FS.Hierarchy;
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
        private V _default;  // TODO: Persist this? Probably not needed.
        private Map<K, V> _map;
        
        public DefaultMap(AnyNode node, V @default) {
            _default = @default;
            _map = new(node);
        }

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

    public static class DefaultMapOps {
        public static void Cap<K, V>(this DefaultMap<K, V> map, K k, V? vMin, V? vMax) 
            where K: IComparable 
            where V: struct, IComparable 
        {
            var val = map[k];
            if (vMin.HasValue && val.CompareTo(vMin.Value) == -1) { val = vMin.Value; }
            if (vMax.HasValue && val.CompareTo(vMax.Value) == 1) { val = vMax.Value; }
            map[k] = val;
        }
    }
}
