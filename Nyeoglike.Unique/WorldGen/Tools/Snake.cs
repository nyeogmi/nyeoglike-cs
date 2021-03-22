using Nyeoglike.Lib;
using Nyeoglike.Unique.WorldGen.Tools.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.WorldGen.Tools {
    public class Snake {
        private Carve _carve;
        private ID<Room> _room;
        private Cardinal _direction;

        public Snake(Carve carve, ID<Room> room, Cardinal direction) {
            _carve = carve;
            _room = room;
            _direction = direction;
        }

        public void VetoPoint(Action<VetoBox> body) {
            var carve = _carve;
            var room = _room;
            var direction = _direction;

            VetoBox box = null;
            // TODO: Use finally here?
            _carve.VetoPoint((b) => {
                box = b;
                body(box);
            });

            if (box?.Vetoed ?? false) {
                _carve = carve;
                _room = room;
                _direction = direction;
            }
        }

        public void Veto() => _carve.Veto();

        public Snake Branch() => new Snake(_carve, _room, _direction);

        public void TurnRight() => _direction = _direction.Right();
        public void TurnLeft() => _direction = _direction.Left();

        public ID<Room> Tunnel(
            V2 size,
            RoomType roomType,
            LinkType linkType,
            int? minContact = null,
            bool useIgnore = false,
            Rule rule = Rule.RNG
        ) {
            if (_direction == Cardinal.East || _direction == Cardinal.West) {
                // swap size so it's left/right
                size = new V2(size.Y, size.X);
            }

            ID<Room> room;
            if (_direction == Cardinal.North) {
                room = _carve.TunnelNorth(_room, size, roomType, minContact, useIgnore, rule);
            }
            else if (_direction == Cardinal.East) {
                room = _carve.TunnelEast(_room, size, roomType, minContact, useIgnore, rule);
            }
            else if (_direction == Cardinal.South) {
                room = _carve.TunnelEast(_room, size, roomType, minContact, useIgnore, rule);
            }
            else if (_direction == Cardinal.West) {
                room = _carve.TunnelEast(_room, size, roomType, minContact, useIgnore, rule);
            }
            else {
                throw new InvalidOperationException($"direction: {_direction}");
            }

            _carve.LinkRooms(linkType, _room, room);
            _room = room;
            return room;
        }
    }
}
