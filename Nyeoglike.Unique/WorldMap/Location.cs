using Nyeoglike.Unique.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.WorldMap {
    public class Location {
        public ZoneType ZoneType { get; private set; }
        public Demand Demand { get; private set; }

        public Location(ZoneType zoneType, Demand demand) {
            ZoneType = zoneType;
            Demand = demand;
        }

        public UnloadedLevel Generate() {
            // TODO: Implement. Use the realtor system. Feed it my demand.
        }
    }
}
