using Nyeoglike.Lib;
using Nyeoglike.Lib.Relations;
using static Nyeoglike.Unique.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyeoglike.Unique.Events;
using Nyeoglike.Lib.Relations.Directional;
using Nyeoglike.Unique.NPCSystems.Scheduling;

namespace Nyeoglike.Unique.NPCSystems {
    public class SceneFlags {
        private ManyToMany<ID<NPC>, SceneFlag> _npcSceneFlags = new();

        public SceneFlags() { }

        public void Add(ID<NPC> id, SceneFlag sceneFlag) {
            if (_npcSceneFlags.Fwd[id].Add(sceneFlag)) {
                // if new
                W.Notify(new Event(Verb.AddFlag, new NPCArg(id), new SceneFlagArg(sceneFlag)));
            }
        }

        public ManyRO<SceneFlag> this[ID<NPC> id] => _npcSceneFlags.Fwd[id].RO;

        public void Reset() {
            _npcSceneFlags = new();
        }

        public void Notify(Event evt) {
            // TODO: Spy on ticks, spy on NPCs to determine if they fell asleep
        }

        public void PopulateFromSchedules() {
            foreach (var npc in W.NPCs.Table.Keys) {
                var schedule = W.Schedules.Previous[npc];

                if (schedule.Type == ScheduleItemType.HomeSleep) {
                    Add(npc, SceneFlag.GotSleep);
                }

                // TODO: Any others?
            }
        }
    }
}
