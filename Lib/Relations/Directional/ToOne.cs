using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Relations.Directional {
    // NOTE: Unlike C#, we use "null" to represent "not found"
    //
    public abstract class ToOne<K, V>
        where K: struct
        where V: struct
    {
        public abstract bool ContainsKey(K k);
        public abstract bool ContainsValue(V v);
        public abstract Nullable<V> this[K k] { get; set; }
        public abstract Nullable<V> Pop(K k);
        
        public bool Remove(K k) { return Pop(k) != null; }
        public bool Remove(K k, out Nullable<V> v) {
            v = Pop(k);
            return v != null;
        }

        public abstract IEnumerator<KeyValuePair<K, V>> GetEnumerator();
        public abstract IEnumerable<K> Keys { get; }
        public abstract IEnumerable<V> Values { get; }

        // TODO: Other Dictionary methods
    }

}
