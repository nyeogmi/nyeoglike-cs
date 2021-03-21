using Nyeoglike.Lib;
using Nyeoglike.Unique.Items;

namespace Nyeoglike.Unique.Level.Elements {
    public class ItemSpawn {
        public ID<ItemSpawn> ID;
        public Item Item;
        public bool Ephemeral;
        public bool IsJunk; // TODO: Property?
    }
}
