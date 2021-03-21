using Nyeoglike.Lib;
using Nyeoglike.Lib.Relations;
using System.Collections.Generic;

namespace Nyeoglike.Unique.Level.Elements {
    public class Wallpaper {
        public WallTile Default;
        private Table<WallTile> _tiles = new();
        private Dictionary<WallTile, ID<WallTile>> _existing = new();
        private OneToOne<V2, ID<WallTile>> _layered = new();

        public Wallpaper(WallTile @default) {
            Default = @default;
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
                return Default;
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
