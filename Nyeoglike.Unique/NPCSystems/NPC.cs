using Nyeoglike.Lib;
using static Nyeoglike.Unique.Globals;

namespace Nyeoglike.Unique.NPCSystems {
    public class NPC {
        public ID<NPC> ID;
        public string Name;
        public bool Seen;  // set in sitemode. TODO: Should this only be tracked there?

        public bool Asleep;  // TODO: Should this be sitemode only?

        public NPC(ID<NPC> id, string name) {
            ID = id;
            Name = name;
        }
    }
}
