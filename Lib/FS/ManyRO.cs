using System.Collections;
using System.Collections.Generic;

namespace Nyeoglike.Lib.FS {
    public class ManyRO<V>: IEnumerable<V> {
        private Many<V> _impl;

        internal ManyRO(Many<V> impl) {
            _impl = impl;
        }

        public bool Contains(V v) => _impl.Contains(v);

        public IEnumerator<V> GetEnumerator() => _impl.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _impl.GetEnumerator();
    }
}
