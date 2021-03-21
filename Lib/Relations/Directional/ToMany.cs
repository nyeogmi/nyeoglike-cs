using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.Relations.Directional {
    public abstract class ToMany<K, V>
        where K: struct
        where V: struct
    {
        public abstract bool ContainsKey(K k);
        public abstract Many<V> this[K k] { get; }
        public abstract SortedSet<V> Pop(K k);
        public bool Remove(K k) { return Pop(k).Any(); }
        public bool Remove(K k, out SortedSet<V> vs) {
            vs = Pop(k);
            return vs.Any();
        }

        public abstract IEnumerator<KeyValuePair<K, V>> GetEnumerator();
        public abstract IEnumerable<K> Keys { get; }
        public abstract IEnumerable<V> Values { get; }

        // TODO: Other Dictionary methods
    }
}
