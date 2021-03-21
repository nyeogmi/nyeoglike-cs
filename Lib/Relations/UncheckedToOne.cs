using Nyeoglike.Lib.Relations.Directional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.Relations {
    public class UncheckedToOne<A, B>
        where A: struct, IComparable // <A>
        where B: struct
    {
        private ulong _tick;
        private SortedDictionary<A, B> _aToB;
        
        public UncheckedToOne() {
            _tick = 0;
            _aToB = new SortedDictionary<A, B>();
        }

        public ViewForward Fwd => new ViewForward(this);

        public bool Add(A a, B b) {
            _tick++;

            if (Contains(a, b)) {
                return false;
            }

            var oldB = GetBFromA(a);
            if (oldB.HasValue) {
                Remove(a, oldB.Value);
            }

            _aToB[a] = b;
            return true;
        }

        public bool Contains(KeyValuePair<A, B> kv) => Contains(kv.Key, kv.Value);
        public bool Contains(A a, B b) {
            if (_aToB.TryGetValue(a, out B oldB)) {
                return oldB.Equals(b);
            }
            return false;
        }

        private bool ContainsA(A a) => _aToB.ContainsKey(a);

        public bool Remove(KeyValuePair<A, B> kv) => Remove(kv.Key, kv.Value);
        public bool Remove(A a, B b) {
            _tick++;
            if (!Contains(a, b)) {
                return false;
            }

            _aToB.Remove(a);
            return true;
        }

        private Nullable<B> PopA(A a) {
            _tick++;
            if (_aToB.Remove(a, out B b)) {
                return b;
            }
            return null;
        }

        private Nullable<B> GetBFromA(A a) {
            if (_aToB.TryGetValue(a, out B b)) {
                return b;
            }
            return null;
        }

        private IEnumerable<KeyValuePair<A, B>> AllAPairs() {
            var _old = _tick;

            foreach (var ab in _aToB) {
                yield return ab;
                if (_tick != _old) {
                    throw new InvalidOperationException("cannot iterate: underlying object changed");
                }
            }
        }

        // TODO: Implement
        // public void Clear();
        // public void ExceptWith(IEnumerable<KeyValuePair<A, B>> other);
        // public void IntersectWith(IEnumerable<KeyValuePair<A, B>> other);
        // public bool IsProperSubsetOf(IEnumerable<KeyValuePair<A, B>> other);
        // public bool IsProperSupersetOf(IEnumerable<KeyValuePair<A, B>> other); 
        // public bool IsSubsetOf(IEnumerable<KeyValuePair<A, B>> other);
        // public bool IsSupersetOf(IEnumerable<KeyValuePair<A, B>> other); 
        // public bool Overlaps(IEnumerable<KeyValuePair<A, B>> other); 
        // public int RemoveWhere(Predicate<KeyValuePair<A, B>> match);
        // public bool SetEquals(IEnumerable<KeyValuePair<A, B>> other);
        // public void SymmetricExceptWith(IEnumerable<KeyValuePair<A, B>> other);
        // public void UnionWith(IEnumerable<KeyValuePair<A, B>> other);

        public class ViewForward: ToOne<A, B> {
            private UncheckedToOne<A, B> _this;

            public ViewForward(UncheckedToOne<A, B> impl) {
                _this = impl;
            }

            public override bool ContainsKey(A a) => _this.ContainsA(a);

            public override Nullable<B> this[A a] {
                get => _this.GetBFromA(a);

                set {
                    if (value.HasValue) {
                        _this.Add(a, value.Value);
                    }
                    else {
                        _this.PopA(a);
                    }
                }
            }

            public override Nullable<B> Pop(A a) => _this.PopA(a);

            public override IEnumerator<KeyValuePair<A, B>> GetEnumerator() {
                foreach (var ab in _this.AllAPairs()) {
                    yield return ab;
                }
            }

            public override IEnumerable<A> Keys {
                get {
                    foreach (var ab in _this.AllAPairs()) {
                        yield return ab.Key;
                    }
                }
            }

            public override IEnumerable<B> Values {
                get {
                    foreach(var ab in _this.AllAPairs()) {
                        yield return ab.Value;
                    }
                }
            }
        }
    }
}
