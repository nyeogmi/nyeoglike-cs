using Nyeoglike.Lib;
using Nyeoglike.Lib.Relations;
using Nyeoglike.Unique.Items;
using Nyeoglike.Unique.Level.Elements;
using Nyeoglike.Unique.NPCSystems;
using Nyeoglike.Unique.NPCSystems.Scheduling;
using static Nyeoglike.Unique.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyeoglike.Unique.WorldMap;
using Nyeoglike.Lib.FS;

namespace Nyeoglike.Unique.Level {
    public class UnloadedLevel {
        public V2 PlayerStart;
        public Terrain Terrain;
        public IxManyMap<V2, Item> Items;
        public ManyToMany<V2, SpawnType> SpawnType;
        public Action<World, LoadedLevel> EphemeraSource;

        public LoadedLevel Load(ID<Location> location) {
            var spawns = (
                from npc in W.NPCs.Table.Keys
                where W.Schedules.Next.GetLocation(npc) == location
                select new SpawnNPC {
                    ID = npc,
                    Schedule = W.Schedules.Next[npc],
                }
            ).ToList();
        }
    }

    struct SpawnNPC { // TODO: Better name. Or factor this out in favor of using the World global.
        public ID<NPC> ID;
        public ScheduleItem Schedule;
    }
}
