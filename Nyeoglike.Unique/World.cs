using Nyeoglike.Lib;
using Nyeoglike.Unique.Biology;
using Nyeoglike.Unique.Events;
using Nyeoglike.Unique.Time;
using Nyeoglike.Unique.WorldMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique {
    public class World {
        public Clock Clock { get; private set; } = new();
        public EventMonitors EventMonitors { get; private set; } = new();
        public Levels Levels { get; private set; } = new();
        public NPCs NPCs { get; private set; } = new();
        public Pushes Pushes { get; private set; } = new();
        public Rhythms Rhythms { get; private set; } = new();

        public LoadedLevel Level { get; private set; } = null;
        public Player Player { get; private set; } = new();

        private bool _notifying = false;
        private Queue<Event> _notifyQueue = new();

        public World() {
        }

        public World Generate() {
            throw new NotImplementedException("TODO");
        }

        public void AdvanceTime() {
            Clock.AdvanceTime();
            EndTimePeriod();
            StartTimePeriod();
        }

        private void StartTimePeriod() {
            if (!Clock.Started) {
                Clock.Start();
                Rhythms.AdvanceTime();
                Schedules.CalculateSchedules();
                SceneFlags.Reset();
                SceneFlags.PopulateFromSchedules();
            }
        }

        private void EndTimePeriod() {
            if (!Clock.Started) {
                return;
            }

            // TODO: Provide rewards for scene flags, like sleep?
            // They should generally be awarded as soon as they are added
        }

        public void FollowNPC(ID<NPC> npc) {
            ActivateLevel(Schedules.NextLocation(npc));
        }

        public void ActivateLocation(ID<Location> location) {
            var spawns = (
                from npc in NPCs.Table.Keys
                where Schedules.NextLocation(npc) == location
                select new SpawnNPC(npc, Schedules.NextSchedule(npc))
            ).ToList();

            var lvl = Levels.Get(location);
            ActivateLevel(location, lvl, spawns);
        }

        private void ActivateLevel(
            ID<Location> location, 
            UnloadedLevel level,
            List<SpawnNPC> npcs
        ) {
            if (Level) {
                // TODO: Save level status
            }

            Player.Pos = level.PlayerStart;
            Player.Cam = level.PlayerStart;
            Level = level.Load(location, npcs);

            // Notify them.
            Challenges.Scan();
        }

        public void Tick() {
            Notify(new Event(Verb.Tick));
        }

        public void Notify(Event evt) {
            _notifyQueue.Enqueue(evt);
            if (_notifying) { return; }

            _notifying = true;
            try {
                while (_notifyQueue.TryDequeue(out evt)) {
                    if (evt.Verb.QuestOnly()) {
                        // No need for NPCs to know. Only quests!
                        EventMonitors.Notify(evt);
                    }
                    else {
                        EventMonitors.Notify(evt);
                        NPCs.Notify(evt);
                        Rhythms.Notify(evt);
                        SceneFlags.Notify(evt);
                    }
                }
            }
            finally {
                _notifying = false;
            }
            throw new NotImplementedException("TODO");
        }
    }
}
