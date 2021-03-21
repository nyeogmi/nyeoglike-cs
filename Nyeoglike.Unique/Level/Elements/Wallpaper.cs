using Nyeoglike.Lib;
using Nyeoglike.Lib.FS;
using Nyeoglike.Lib.FS.Hierarchy;
using Nyeoglike.Lib.Relations;
using System.Collections.Generic;

namespace Nyeoglike.Unique.Level.Elements {
    public class Wallpaper {
        public Box<WallTile> Default;
        private Table<WallTile> _tiles;
        private Map<WallTile, ID<WallTile>> _existing;
        private OneToOne<V2, ID<WallTile>> _layered;

        public Wallpaper(Node<Temporary> node, WallTile @default) {
            Default = new(node.Sub("wallpaper")); Default.Default(@default);
            _tiles = new(node.Sub("tiles"));
            _existing = new(node.Sub("existing"));
            _layered = new(node.Sub("layered"));
        }

        public void Add(WallTile wt, IEnumerable<V2> cells) {
            var id = ID(wt);

            foreach (var c in cells) {
                _layered.Fwd[c] = id;
            }
        }

        public WallTile this[V2 v] {
            get {
                var here = _layered.Fwd[v];
                if (here.HasValue) { return _tiles[here.Value]; }
                return Default.X;
            }
            set {
                Add(value, new[] { v });
            }
        }

        private ID<WallTile> ID(WallTile wt) {
            if (_existing.TryGetValue(wt, out ID<WallTile> id)) {
                return id;
            }

            id = _tiles.Add(wt);
            _existing[wt] = id;
            return id;
        }
    }
}
