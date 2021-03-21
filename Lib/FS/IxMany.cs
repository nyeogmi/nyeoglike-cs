using System.Collections;
using System.Collections.Generic;

namespace Nyeoglike.Lib.FS {
    public abstract class IxMany<V>: IEnumerable<V> {
        public abstract IEnumerator<V> GetEnumerator();

        // TODO: Other List and Dictionary methods
        public abstract void Add(V v);
        public abstract void Insert(int i, V v);
        public abstract bool Contains(int iv);
        public abstract bool Remove(int iv);
        public abstract bool Remove(int iv, out V v);

        public abstract V this[int i] { get; set; }
        public abstract bool TryGetValue(int i, out V v);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IxManyRO<V> RO => new(this);

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
