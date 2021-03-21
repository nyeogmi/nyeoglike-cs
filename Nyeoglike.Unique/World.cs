using Nyeoglike.Lib;
using Nyeoglike.Unique.Events;
using Nyeoglike.Unique.Level;
using Nyeoglike.Unique.NPCSystems;
using Nyeoglike.Unique.NPCSystems.Capitalism;
using Nyeoglike.Unique.NPCSystems.Scheduling;
using Nyeoglike.Unique.PlayerSystems;
using Nyeoglike.Unique.QuestSystem;
using Nyeoglike.Unique.Time;
using Nyeoglike.Unique.WorldMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique {
    public class World {
        public Challenges Challenges { get; private set; } = new();
        public Clock Clock { get; private set; } = new();
        public Enterprises Enterprises { get; private set; } = new();
        public EventMonitors EventMonitors { get; private set; } = new();
        public Levels Levels { get; private set; } = new();
        public NameGen NameGen { get; private set; } = new();
        public NPCs NPCs { get; private set; } = new();
        public Pushes Pushes { get; private set; } = new();
        public Rhythms Rhythms { get; private set; } = new();
        public RNG RNG { get; private set; } = new();
        public Schedules Schedules { get; private set; } = new();
        public SceneFlags SceneFlags { get; private set; } = new();

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
                Schedules.Calculate();
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
            ActivateLocation(Schedules.Next.GetLocation(npc));
        }

        public void ActivateLocation(ID<Location> location) {
            var level = Levels.Get(location);

            if (Level != null) {
                // TODO: Save previous level's status
            }

            Player.Pos = level.PlayerStart;
            Player.Cam = level.PlayerStart;
            Level = level.Load(location);

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
