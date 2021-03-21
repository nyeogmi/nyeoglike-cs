using Nyeoglike.Lib.FS;
using Nyeoglike.Lib.Relations.Directional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.Relations {
    // TODO: EdgeManyToMany<A, E, B>
    // Implement it as a ManyToMany<A, (B, E)> next to a ManyToMany<(A, E), B>

    public class ManyToMany<A, B>
        where A: struct, IComparable // <A>
        where B: struct, IComparable // <B>
    {
        private ulong _tick = 0;
        private ManyMap<A, B> _aToBs = new();
        private ManyMap<B, A> _bToAs = new();
        
        public ManyToMany() {
        }

        public ViewForward Fwd => new ViewForward(this);
        public ViewReverse Rev => new ViewReverse(this);

        public bool Add(A a, B b) {
            _tick++;

            if (Contains(a, b)) {
                return false;
            }

            _aToBs.Add(a, b);
            _bToAs.Add(b, a);

            return true;
        }

        public bool Contains(KeyValuePair<A, B> kv) => Contains(kv.Key, kv.Value);
        public bool Contains(A a, B b) => _aToBs.Contains(a, b);

        private bool ContainsA(A a) => _aToBs.ContainsKey(a);
        private bool ContainsB(B b) => _bToAs.ContainsKey(b);

        public bool Remove(KeyValuePair<A, B> kv) => Remove(kv.Key, kv.Value);
        public bool Remove(A a, B b) {
            _tick++;
            if (!Contains(a, b)) {
                return false;
            }

            _aToBs.Remove(a, b);
            _bToAs.Remove(b, a);

            return true;
        }

        private SortedSet<B> PopA(A a) {
            _tick++;
            var bs = _aToBs.PopKey(a);
            foreach (var b in bs) {
                _bToAs.Remove(b, a);
            }
            return bs;
        }

        private SortedSet<A> PopB(B b) {
            _tick++;
            var as_ = _bToAs.PopKey(b);
            foreach (var a in as_) {
                _aToBs.Remove(a, b);
            }
            return as_;
        }

        private Many<B> GetBsFromA(A a) => new ViewManyForward(this, a);
        private Many<A> GetAsFromB(B b) => new ViewManyReverse(this, b);

        private IEnumerable<B> AllBsFromA(A a) {
            var _old = _tick;
            foreach (var b in _aToBs[a]) {
                yield return b;
                if (_tick != _old) {
                    throw new InvalidOperationException("cannot iterate: underlying object changed");
                }
            }
        }

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

            foreach (var ab in _aToBs) {
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

        public class ViewForward: ToMany<A, B> {
            private ManyToMany<A, B> _this;

            public ViewForward(ManyToMany<A, B> impl) {
                _this = impl;
            }

            public override bool ContainsKey(A a) => _this.ContainsA(a);
            public bool ContainsValue(B b) => _this.ContainsB(b);

            public override Many<B> this[A a] => _this.GetBsFromA(a);

            public override SortedSet<B> Pop(A a) => _this.PopA(a);

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
                    foreach (var ab in _this.AllAPairs()) {
                        yield return ab.Value;
                    }
                }
            }
        }

        public class ViewReverse: ToMany<B, A> {
            private ManyToMany<A, B> _this;

            public ViewReverse(ManyToMany<A, B> impl) {
                _this = impl;
            }

            public override bool ContainsKey(B b) => _this.ContainsB(b);
            public bool ContainsValue(A a) => _this.ContainsA(a);

            public override Many<A> this[B b] => _this.GetAsFromB(b);

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

        public class ViewManyForward : Many<B> {
            private ManyToMany<A, B> _this;
            private A _a;

            public ViewManyForward(ManyToMany<A, B> impl, A a) {
                _this = impl;
                _a = a;
            }

            public override bool Add(B b) => _this.Add(_a, b);
            public override bool Contains(B b) => _this.Contains(_a, b);
            public override bool Remove(B b) => _this.Remove(_a, b);

            public override IEnumerator<B> GetEnumerator() {
                foreach (var b in _this.AllBsFromA(_a)) {
                    yield return b;
                }
            }
        }

        public class ViewManyReverse : Many<A> {
            private ManyToMany<A, B> _this;
            private B _b;

            public ViewManyReverse(ManyToMany<A, B> impl, B b) {
                _this = impl;
                _b = b;
            }

            public override bool Add(A a) => _this.Add(a, _b);
            public override bool Contains(A a) => _this.Contains(a, _b);
            public override bool Remove(A a) => _this.Remove(a, _b);

            public override IEnumerator<A> GetEnumerator() {
                foreach (var a in _this.AllAsFromB(_b)) {
                    yield return a;
                }
            }
        }
    }
}
