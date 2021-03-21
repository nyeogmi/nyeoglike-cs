using Nyeoglike.Lib;
using Nyeoglike.Unique.Events;
using static Nyeoglike.Unique.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyeoglike.Lib.FS;

namespace Nyeoglike.Unique.NPCSystems {
    public class NPCs {
        public Table<NPC> Table; 
        private Map<string, bool> _usedNames;

        public NPCs() {
            var root = S.Root("npcs");
            Table = new(root.Sub("table"));
            _usedNames = new(root.Sub("usedNames"));
        }

        public ID<NPC> Generate() {
            var id = Table.Add((id) => new NPC(id, W.NameGen.Generate()));
            _usedNames[Table[id].Name] = true;
            return id;
        }

        public void Notify(Event evt) {
            throw new ArgumentException("TODO");
        }
    }
}
