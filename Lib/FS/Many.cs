using System.Collections;
using System.Collections.Generic;

namespace Nyeoglike.Lib.FS {
    public abstract class Many<V>: IEnumerable<V> {
        public abstract IEnumerator<V> GetEnumerator();

        // TODO: Other Set methods
        public abstract bool Add(V v);
        public abstract bool Contains(V v);
        public abstract bool Remove(V v);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public ManyRO<V> RO => new(this);

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
