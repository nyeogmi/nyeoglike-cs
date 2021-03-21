using Nyeoglike.Lib;
using Nyeoglike.Lib.Relations;
using Nyeoglike.Unique.Level.Elements;
using Nyeoglike.Unique.NPCSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Level {
    public class LoadedLevel {
        public ID<UnloadedLevel> ID;
        public Terrain Terrain;
        public ItemSpawns Items;
        public ManyToOne<ID<NPC>, V2> NPCLocation;
    }
}
