using Nyeoglike.Lib;
using Nyeoglike.Lib.FS;
using Nyeoglike.Lib.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Level.Elements {
    public class Terrain {
        public Wallpaper Wallpaper;
        public SortedSet<V2> InBounds;
        public Map<V2, Block> Blocks;
    }
}
