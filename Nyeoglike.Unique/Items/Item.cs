using Nyeoglike.Lib.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Items {
    public struct Item {
        public Profile Profile;
        public bool OccludesNPCSpawn;
        public int BuyPrice;
        public FrozenSet<string> Keywords;
        public FrozenSet<Contribution> Contributions;

        // TODO: Constructors
    }
}
