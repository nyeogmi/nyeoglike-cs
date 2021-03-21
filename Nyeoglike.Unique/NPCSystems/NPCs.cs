using Nyeoglike.Lib;
using Nyeoglike.Unique.Events;
using static Nyeoglike.Unique.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.NPCSystems {
    public class NPCs {
        public Table<NPC> Table = new();
        private SortedSet<string> _usedNames = new SortedSet<string>();

        public NPCs() {

        }

        public ID<NPC> Generate() =>
            Table.Add((id) => new NPC(id, W.NameGen.Generate()));

        public void Notify(Event evt) {
            throw new ArgumentException("TODO");
        }
    }
}
