using Nyeoglike.Lib.Relations;
using Nyeoglike.Unique.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.PlayerSystems {
    public class Inventory {
        private UncheckedToOne<Resource, int> _resources = new(); 

        public Inventory() {
            _resources[]
        }
    }
}
