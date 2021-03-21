using Nyeoglike.Lib;
using Nyeoglike.Lib.FS;
using Nyeoglike.Lib.FS.Hierarchy;
using Nyeoglike.Lib.Relations;
using Nyeoglike.Unique.Events;
using Nyeoglike.Unique.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Nyeoglike.Unique.Globals;

namespace Nyeoglike.Unique.PlayerSystems {
    public class Inventory {
        private DefaultMap<Resource, int> _resources; // TODO: Move this to its own object
        public Claims Claims;

        public Inventory() {
            // for now
            var root = S.Root("inventory");
            _resources = new(root.Sub("resources"), 0);
            _resources[Resource.Money] = 10000;

            Claims = new Claims(root.Sub("claims"));
        }

        public void Add(Item item) {
            var box = new ClaimBox(item, false);
            W.Notify(new Event(Verb.Claim, new ClaimBoxArg(box)));

            if (!box.Taken) { Liquidate(item); }
        }

        public void Liquidate(Item item) {
            foreach (var c in item.Contributions) {
                _resources[c.Resource] += c.N;
            }

            _resources.Cap(Resource.Blood, 0, 100);
            _resources.Cap(Resource.Spark, 0, 100);
        }

        public int this[Resource r] => _resources[r];
        public bool Take(Resource r, int n) {
            if (n == 0) {
                return true;
            }

            if (n > _resources[r]) {
                return false;
            }

            _resources[r] -= n;
            return true;
        }
    }

    public class Claims {
        private DropTable<Claim> _table;
        private OneToMany<ID<EventMonitor>, ID<Claim>> _questClaims;

        public Claims(Node<Permanent> node) {
            var root = node.Sub("claims");
            _table = new(root);
            _questClaims = new(root.Sub("questClaims"));
        }

        public ID<Claim> Claim(ID<EventMonitor> quest, Item item) {
            var id = _table.Add(new Claim { Item = item });
            _questClaims.Fwd[quest].Add(id);
            return id;
        }

        public Item Redeem(ID<Claim> claim) {
            _questClaims.Rev.Pop(claim);
            if (_table.Remove(claim, out Claim c)) {
                return c.Item;
            }
            throw new InvalidOperationException($"claim no longer exists: {claim}");
        }

        // TODO: When a quest is canceled, close its claims and liquidate the items
    }

    public class Claim {
        public Item Item;
    }

    public class ClaimBox {
        public Item Item { get; private set; }
        public bool Taken { get; private set; }
        private bool _hypothetical;

        public ClaimBox(Item item, bool hypothetical) {
            Item = item;
            Taken = false;
            _hypothetical = hypothetical;
        }

        public ID<Claim> Claim(ID<EventMonitor> quest) {
            if (_hypothetical) {
                throw new HypotheticalClaimException(quest);
            }

            var handle = W.Inventory.Claims.Claim(quest, Item);
            Taken = true;
            return handle;
        }
    }

    public class HypotheticalClaimException: Exception {
        public ID<EventMonitor> Claimant { get; private set; }

        public HypotheticalClaimException(ID<EventMonitor> claimant) {
            Claimant = claimant;
        }
    }
}
