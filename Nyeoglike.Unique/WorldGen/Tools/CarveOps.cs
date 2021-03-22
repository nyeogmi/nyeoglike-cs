using Nyeoglike.Lib;
using Nyeoglike.Unique.WorldGen.Tools.CarveOp;
using Nyeoglike.Unique.WorldGen.Tools.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.WorldGen.Tools {
    public partial class Carve { 
        // == Carve ops (undo-able) ==
        private void AddHint(V2 v2, Hint hint) {
            if (_hints.Fwd[v2].Contains(hint)) { return; }

            _hints.Fwd[v2].Add(hint);
            _operationLog.Add(new AddHint { Position = v2, Hint = hint });
        }

        private void CarveTile(V2 v2, ID<Room>? owner) {
            var oldOwner = _tiles.Rev[v2];
            _tiles.Rev[v2] = owner;

            _operationLog.Add(new CarveTile { Position = v2, OldOwner = oldOwner });
        }

        private ID<Room> CreateRoom(RoomType type) {
            var id = _rooms.Add(new Room { Type = type, Frozen = false });

            _operationLog.Add(new CreateRoom { Room = id });
            return id;
        }

        private void FreezeRoom(ID<Room> room) {
            if (_rooms[room].Frozen) { return; }
            _rooms[room].Frozen = true;

            _operationLog.Add(new FreezeRoom { Room = room });
        }

        internal void LinkRooms(LinkType type, ID<Room> room0, ID<Room> room1) {  // expose for snake
            var id = _links.Add(new Link { Type = type, Room0 = room0, Room1 = room1 });

            _operationLog.Add(new LinkRooms { Link = id });
        }

        private void UndoOp(ICarveOp op) {
            if (op is AddHint ah) {
                _hints.Fwd[ah.Position].Remove(ah.Hint);
            }
            else if (op is CarveTile ct) {
                _tiles.Rev[ct.Position] = ct.OldOwner;
            }
            else if (op is CreateRoom cr) {
                _rooms.Remove(cr.Room);
                _tiles.Fwd.Remove(cr.Room);
            }
            else if (op is FreezeRoom fr) {
                _rooms[fr.Room].Frozen = false;
            }
            else if (op is LinkRooms lr) {
                _links.Remove(lr.Link);
            }
            else {
                throw new InvalidOperationException($"can't undo: {op}");
            }
        }
        // == End carve ops ==
    }
}
