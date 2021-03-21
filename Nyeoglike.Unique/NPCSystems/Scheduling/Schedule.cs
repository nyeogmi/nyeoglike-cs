using Nyeoglike.Lib;
using Nyeoglike.Lib.FS;
using Nyeoglike.Lib.FS.Hierarchy;
using Nyeoglike.Lib.Relations;
using Nyeoglike.Unique.Level;
using Nyeoglike.Unique.WorldMap;
using System;

namespace Nyeoglike.Unique.NPCSystems.Scheduling {
    public class Schedule {
        private Store<Temporary> _store;
        private NullMap<ID<NPC>, ScheduleItem> _items;
        private OneToOne<ID<NPC>, ID<UnloadedLevel>> _calculatedLocation;
        private OneToMany<ID<NPC>, ID<NPC>> _calculatedLocationDetermines;

        public Schedule() {
            _store = new();
            var root = _store.Root("schedule");
            _items = new(root.Sub("items"));
            _calculatedLocation = new(root.Sub("calculatedLocation"));
            _calculatedLocationDetermines = new(root.Sub("calculatedLocationDetermines"));
        }

        // TODO: Do something smart when these fail
        public ScheduleItem this[ID<NPC> id] {
            get => _items[id].Value;
            set {
                Remove(id);
                _items[id] = value;
            }
        }

        public void Remove(ID<NPC> id) {
            ClearLocation(id);
        }

        public ID<Location> GetLocation(ID<NPC> id) {
            throw new NotImplementedException("TODO: Port this");
        }

        public void SetLocation(ID<NPC> id, ID<Location> level) {
            throw new NotImplementedException("TODO: Port this");
        }

        public void ClearLocation(ID<NPC> id) {
            throw new NotImplementedException("TODO: Port this");
        }
    }
}
