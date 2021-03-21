using Nyeoglike.Lib;
using Nyeoglike.Unique.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.WorldMap {
    public class Levels {
        private Table<Location> Table = new();
        private SortedDictionary<ID<Location>, UnloadedLevel> _generation = new();
        private SortedDictionary<ZoneType, Realtor> _realtors = new();

        // TODO: Realtors

        public Levels() {
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
