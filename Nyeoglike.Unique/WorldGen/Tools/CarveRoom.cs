using Nyeoglike.Lib;
using Nyeoglike.Lib.Immutable;
using Nyeoglike.Unique.WorldGen.Tools.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.WorldGen.Tools {
    public partial class Carve {
        public ID<Room> CarveRoom(R2 r, RoomType roomType, FrozenSet<ID<Room>> ignore) {
            var id = CreateRoom(roomType);

            var affectedRooms = new SortedDictionary<ID<Room>, int>();

            foreach (var v in r.Expand(new V2(1, 1))) {
                var existingRoom = _tiles.Rev[v];

                if (existingRoom is ID<Room> room) {
                    if (ignore.Contains(room)) { continue; }
                    if (_rooms[room].Frozen) { Veto(); }
                    affectedRooms[room] = _tiles.Fwd[room].Count;
                }

                CarveTile(v, r.Contains(v) ? id : null);
            }

            foreach (var kv in affectedRooms) {
                var room = kv.Key;
                var oldArea = kv.Value;
                var newArea = _tiles.Fwd[room].Count;

                if (Ruined(room, oldArea, newArea)) {
                    Veto();
                }
            }

            return id;
        }

        private bool Ruined(ID<Room> room, int previousArea, int newArea) {
            if (_rooms[room].Frozen) { return true; }
            if (newArea < 0.5 * previousArea) { return true; }
            if (previousArea > 6 && newArea < 6) { return true; }

            // TODO: Check for no longer contiguous
            // TODO: Check for wonky shape

            return false;
        }
    }
}
