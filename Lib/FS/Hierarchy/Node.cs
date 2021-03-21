﻿namespace Nyeoglike.Lib.FS.Hierarchy {
    public class Node<Phantom> : AnyNode where Phantom : IDuration {
        private Store<Phantom> _store;
        private string _path;

        public Node(Store<Phantom> store, string path) {
            _store = store;
            _path = path;
        }

        public Node<Phantom> Sub(string path) => new Node<Phantom>(_store, $"{_path}.{path}");
        public AnyNode GenericSub(string path) => Sub(path);
        public void Bind(IPrimitive primitive) {
            _store.Bind(_path, primitive);
        }
    }
}