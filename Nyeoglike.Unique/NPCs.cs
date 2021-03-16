using Nyeoglike.Lib;
using Nyeoglike.Unique.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique {
    public class NPCs {
        public Table<NPC> Table = new();
        private SortedSet<string> _usedNames = new SortedSet<string>();

        public NPCs() {

        }

        public ID<NPC> Generate() => Table.Add((handle) => NPC.Generate(handle));

        public void Notify(Event evt) {
            throw new ArgumentException("TODO");
        }
    }

    public class NPC {
        public static NPC Generate(ID<NPC> id) {
            // TODO
        }
    }
}
