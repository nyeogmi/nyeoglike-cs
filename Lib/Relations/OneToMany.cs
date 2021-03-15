using Lib.Relations.Directional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Relations {
    public class OneToMany<A, B>
        where A: struct, IComparable<A>
        where B: struct, IComparable<B>
    {
        private ulong _tick;
        private SortedDictionary<A, SortedSet<B>> _aToBs;
        private SortedDictionary<B, A> _bToA;
        
        public OneToMany() {
            _tick = 0;
            _aToBs = new SortedDictionary<A, SortedSet<B>>();
            _bToA = new SortedDictionary<B, A>();
        }

        public ViewForward Fwd => new ViewForward(this);
        public ViewReverse Rev => new ViewReverse(this);

        public bool Add(A a, B b) {
            _tick++;

            if (Contains(a, b)) {
                return false;
            }

            var oldA = GetAFromB(b);
            if (oldA.HasValue) {
                Remove(oldA.Value, b);
            }

            _bToA[b] = a;
            if (_aToBs.TryGetValue(a, out SortedSet<B> bs)) {
                bs.Add(b);
            }
            else {
                bs = new SortedSet<B>();
                bs.Add(b);
                _aToBs[a] = bs;
            }
            return true;
        }

        public bool Contains(KeyValuePair<A, B> kv) => Contains(kv.Key, kv.Value);
        public bool Contains(A a, B b) { 
            if (_bToA.TryGetValue(b, out A oldA)) {
                return oldA.Equals(a);
            }
            return false;
        }

        private bool ContainsA(A a) => _aToBs.ContainsKey(a);
        private bool ContainsB(B b) => _bToA.ContainsKey(b);

        public bool Remove(KeyValuePair<A, B> kv) => Remove(kv.Key, kv.Value);
        public bool Remove(A a, B b) {
            _tick++;
            if (!Contains(a, b)) {
                return false;
            }

            var bs = _aToBs[a];
            bs.Remove(b);
            if (!bs.Any()) {
                _aToBs.Remove(a);
            }
            _bToA.Remove(b);
            return true;
        }

        private SortedSet<B> PopA(A a) {
            _tick++;
            if (_aToBs.Remove(a, out SortedSet<B> bs)) {
                return bs;
            }
            return new SortedSet<B>();
        }

        private Nullable<A> PopB(B b) {
            _tick++;
            if (_bToA.Remove(b, out A a)) {
                return a;
            }
            return null;
        }

        private Many<B> GetBsFromA(A a) => new ViewManyForward(this, a);
        private Nullable<A> GetAFromB(B b) {
            if (_bToA.TryGetValue(b, out A a)) {
                return a;
            }
            return null;
        }

        private IEnumerable<B> AllBsFromA(A a) {
            var _old = _tick;
            if (_aToBs.TryGetValue(a, out SortedSet<B> bs)) {
                foreach (var b in bs) {
                    yield return b;
                    if (_tick != _old) {
                        throw new InvalidOperationException("cannot iterate: underlying object changed");
                    }
                }
            }
        }

        private IEnumerable<KeyValuePair<A, B>> AllAPairs() {
            var _old = _tick;

            foreach (var abs in _aToBs) {
                foreach (var b in abs.Value) {
                    yield return new KeyValuePair<A, B>(abs.Key, b);
                    if (_tick != _old) {
                        throw new InvalidOperationException("cannot iterate: underlying object changed");
                    }
                }
            }
        }

        private IEnumerable<KeyValuePair<B, A>> AllBPairs() {
            var _old = _tick;

            foreach (var ba in _bToA) {
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
            private OneToMany<A, B> _this;

            public ViewForward(OneToMany<A, B> impl) {
                _this = impl;
            }

            public override bool ContainsKey(A a) => _this.ContainsA(a);
            public override bool ContainsValue(B b) => _this.ContainsB(b);

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

        public class ViewReverse: ToOne<B, A> {
            private OneToMany<A, B> _this;

            public ViewReverse(OneToMany<A, B> impl) {
                _this = impl;
            }

            public override bool ContainsKey(B b) => _this.ContainsB(b);
            public override bool ContainsValue(A a) => _this.ContainsA(a);

            public override Nullable<A> this[B b] {
                get => _this.GetAFromB(b);

                set {
                    if (value.HasValue) {
                        _this.Add(value.Value, b);
                    }
                    else {
                        _this.PopB(b);
                    }
                }
            }

            public override Nullable<A> Pop(B b) => _this.PopB(b);

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

        public class ViewManyForward: Many<B> {
            private OneToMany<A, B> _this;
            private A _a;

            public ViewManyForward(OneToMany<A, B> impl, A a) {
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
    }
}
