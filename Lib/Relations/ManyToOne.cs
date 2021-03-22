using Nyeoglike.Lib.FS;
using Nyeoglike.Lib.FS.Hierarchy;
using Nyeoglike.Lib.Relations.Directional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.Relations {
    public class ManyToOne<A, B>
        where A: struct, IComparable // <A>
        where B: struct, IComparable // <B>
    {
        private ulong _tick;
        private Map<A, B> _aToB;
        private ManyMap<B, A> _bToAs;
        
        public ManyToOne(AnyNode node) {
            _tick = 0;
            _aToB = new(node.GenericSub("fwd"));
            _bToAs = new(node.GenericSub("rev"));
        }

        public ViewForward Fwd => new ViewForward(this);
        public ViewReverse Rev => new ViewReverse(this);

        public bool Add(A a, B b) {
            _tick++;

            if (Contains(a, b)) {
                return false;
            }

            var oldB = GetBFromA(a);
            if (oldB.HasValue) {
                Remove(a, oldB.Value);
            }

            _aToB.Add(a, b);
            _bToAs.Add(b, a);

            return true;
        }

        public bool Contains(KeyValuePair<A, B> kv) => Contains(kv.Key, kv.Value);
        public bool Contains(A a, B b) => _aToB.Contains(a, b); 

        private bool ContainsA(A a) => _aToB.ContainsKey(a);
        private bool ContainsB(B b) => _bToAs.ContainsKey(b);

        public bool Remove(KeyValuePair<A, B> kv) => Remove(kv.Key, kv.Value);
        public bool Remove(A a, B b) {
            _tick++;
            if (!Contains(a, b)) {
                return false;
            }

            _aToB.Remove(a);
            _bToAs.Remove(b, a);

            return true;
        }

        private Nullable<B> PopA(A a) {
            _tick++;
            if (_aToB.Remove(a, out B b)) {
                return b;
            }
            return null;
        }

        private SortedSet<A> PopB(B b) {
            _tick++;
            var as_ = _bToAs.PopKey(b);
            foreach (var a in as_) {
                _aToB.Remove(a);
            }
            return as_;
        }

        private Nullable<B> GetBFromA(A a) {
            if (_aToB.TryGetValue(a, out B b)) {
                return b;
            }
            return null;
        }

        private Many<A> GetAsFromB(B b) => new ViewManyReverse(this, b);

        private int CountAsFromB(B b) => _bToAs[b].Count;
        private IEnumerable<A> AllAsFromB(B b) {
            var _old = _tick;
            foreach (var a in _bToAs[b]) {
                yield return a;
                if (_tick != _old) {
                    throw new InvalidOperationException("cannot iterate: underlying object changed");
                }
            }
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

        private IEnumerable<KeyValuePair<B, A>> AllBPairs() {
            var _old = _tick;

            foreach (var ba in _bToAs) {
                yield return ba;
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
            private ManyToOne<A, B> _this;

            public ViewForward(ManyToOne<A, B> impl) {
                _this = impl;
            }

            public override bool ContainsKey(A a) => _this.ContainsA(a);
            public bool ContainsValue(B b) => _this.ContainsB(b);

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

        public class ViewReverse: ToMany<B, A> {
            private ManyToOne<A, B> _this;

            public ViewReverse(ManyToOne<A, B> impl) {
                _this = impl;
            }

            public override bool ContainsKey(B b) => _this.ContainsB(b);
            public bool ContainsValue(A a) => _this.ContainsA(a);

            public override Many<A> this[B b] => new ViewManyReverse(_this, b);

            public override SortedSet<A> Pop(B b) => _this.PopB(b);

            public override IEnumerator<KeyValuePair<B, A>> GetEnumerator() {
                foreach (var ba in _this.AllBPairs()) {
                    yield return ba;
                }
            }

            public override IEnumerable<B> Keys {
                get {
                    foreach (var ba in _this.AllBPairs()) {
                        yield return ba.Key;
                    }
                }
            }

            public override IEnumerable<A> Values {
                get {
                    foreach (var ba in _this.AllBPairs()) {
                        yield return ba.Value;
                    }
                }
            }
        }

        public class ViewManyReverse : Many<A> {
            private ManyToOne<A, B> _this;
            private B _b;

            public ViewManyReverse(ManyToOne<A, B> impl, B b) {
                _this = impl;
                _b = b;
            }

            public override bool Add(A a) => _this.Add(a, _b);
            public override bool Contains(A a) => _this.Contains(a, _b);
            public override bool Remove(A a) => _this.Remove(a, _b);
            public override int Count => _this.CountAsFromB(_b);

            public override IEnumerator<A> GetEnumerator() {
                foreach (var a in _this.AllAsFromB(_b)) {
                    yield return a;
                }
            }
        }
    }
}
