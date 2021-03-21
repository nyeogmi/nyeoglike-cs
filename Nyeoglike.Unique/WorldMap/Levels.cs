using Nyeoglike.Lib;
using Nyeoglike.Lib.FS;
using Nyeoglike.Unique.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Nyeoglike.Unique.Globals;

namespace Nyeoglike.Unique.WorldMap {
    public class Levels {
        private Table<Location> Table;
        private Map<ID<Location>, UnloadedLevel> _generation;
        private Map<ZoneType, Realtor> _realtors;

        // TODO: Realtors

        public Levels() {
            var root = S.Root("levels");
            Table = new(root);
            _generation = new(root.Sub("generation"));
            _realtors = new(root.Sub("realtors"));
        }

        // TODO: Don't use Zone, just use Table.Add.

        public UnloadedLevel Get(ID<Location> l) {
            if (!_generation.ContainsKey(l)) {
                var location = Table[l];
                _generation[l] = location.Generate();
            }
            return _generation[l];
        }
    }
}
