using Nyeoglike.Lib;
using Nyeoglike.Lib.Relations;
using Nyeoglike.Unique.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Level.Elements {
    public class ItemSpawns {
        private DropTable<ItemSpawn> _table;
        private OneToMany<V2, ID<ItemSpawn>> _placement;
        public int JunkLeft;  // TODO: Property?

        public ID<ItemSpawn> Put(V2 v, Item item, bool ephemeral) {
            // TODO: Calculate IsJunk? (see loaded_level.py)
            var isJunk = ephemeral && item.Keywords.Contains("junk");
            var id = _table.Add((id) => new ItemSpawn { 
                ID = id, Item = item, Ephemeral = ephemeral, 
                IsJunk = isJunk,
            });
            _placement.Fwd[v].Add(id);
            if (isJunk) { JunkLeft++; }

            return id;
        }

        public bool ContainsKey(ID<ItemSpawn> id) => _table.ContainsKey(id);
        public Item Take(ID<ItemSpawn> id) {
            if (!_table.Remove(id, out ItemSpawn spawn)) {
                throw new InvalidOperationException("can't take a spawn that no longer exists");
            }

            if (spawn.IsJunk) { JunkLeft--; }
            return spawn.Item;
        }

        // TODO: Function to view spawns in a cell. Use ROMany
    }
}
