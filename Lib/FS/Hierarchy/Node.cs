using System;

namespace Nyeoglike.Lib.FS.Hierarchy {
    public static class Node {
        public static Node<Temporary> Free => new(null, null);
    }

    public class Node<Phantom> : AnyNode where Phantom : IDuration {
        private Store<Phantom> _store;
        private string _path;

        public Node(Store<Phantom> store, string path) {
            _store = store;
            _path = path;
        }

        public Node<Phantom> Sub(string path) => new Node<Phantom>(_store, $"{_path}.{path}");
        public AnyNode GenericSub(string path) => Sub(path);
        public void Bind(IPrimitive primitive) => Bind(primitive, null); 
        public void Bind(IPrimitive primitive, Action onLoad) {
            if (_store == null) { return; }
            _store.Bind(_path, primitive, onLoad);
        }
    }
}
