using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Relations.Directional {
    public abstract class ToMany<K, V>
        where K: struct
        where V: struct
    {
        public abstract bool ContainsKey(K k);
        public abstract bool ContainsValue(V v);
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

    public abstract class Many<V> {
        public abstract IEnumerator<V> GetEnumerator();

        // TODO: Other Set methods
        public abstract bool Add(V v);
        public abstract bool Contains(V v);
        public abstract bool Remove(V v);

        // TODO: Implement
        // public void Clear();
        // public void ExceptWith(IEnumerable<V> other);
        // public void IntersectWith(IEnumerable<V> other);
        // public bool IsProperSubsetOf(IEnumerable<V> other);
        // public bool IsProperSupersetOf(IEnumerable<V> other); 
        // public bool IsSubsetOf(IEnumerable<V> other);
        // public bool IsSupersetOf(IEnumerable<V> other); 
        // public bool Overlaps(IEnumerable<V> other); 
        // public int RemoveWhere(Predicate<V> match);
        // public bool SetEquals(IEnumerable<V> other);
        // public void SymmetricExceptWith(IEnumerable<V> other);
        // public void UnionWith(IEnumerable<V> other);
    }
}
