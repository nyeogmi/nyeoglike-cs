using Nyeoglike.Lib.FS;
using Nyeoglike.Lib.Relations;
using Nyeoglike.Unique.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.PlayerSystems {
    public class Inventory {
        private DefaultMap<Resource, int> _resources = new(0); 

        public Inventory() {
            // for now
            _resources[Resource.Money] = 10000;
        }
    }
}
