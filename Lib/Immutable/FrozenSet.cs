using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.Immutable {
    public class FrozenSet<T>: IEnumerable<T> where T: IComparable {
        private SortedSet<T> _underlying;
        private T[] _array;

        public FrozenSet(IEnumerable<T> items) {
            _underlying = new SortedSet<T>(items);
            _array = _underlying.ToArray();
        }

        public FrozenSet(params T[] items) {
            _underlying = new SortedSet<T>(items);
            _array = _underlying.ToArray();
        }

        // TODO: Provide methods that usually would mutate
        public bool Contains(T t) => _underlying.Contains(t);

        public IEnumerator<T> GetEnumerator() {
            foreach (var t in _underlying) {
                yield return t;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var t in _underlying) {
                yield return t;
            }
        }

        // mostly to be used by RNG
        public T Choice(Random r) => _array[r.Next(_array.Length)];
    }
}
