using System.Collections;
using System.Collections.Generic;

namespace Nyeoglike.Lib.FS {
    public class IxManyRO<V>: IEnumerable<V> {
        private IxMany<V> _impl;

        internal IxManyRO(IxMany<V> impl) {
            _impl = impl;
        }

        public bool Contains(int iv) => _impl.Contains(iv);

        public V this[int i] => _impl[i];
        public bool TryGetValue(int i, out V v) => _impl.TryGetValue(i, out v);

        public IEnumerator<V> GetEnumerator() => _impl.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _impl.GetEnumerator();
    }
}
