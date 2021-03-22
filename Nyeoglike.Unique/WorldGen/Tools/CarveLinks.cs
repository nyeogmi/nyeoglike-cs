using Nyeoglike.Lib;
using Nyeoglike.Lib.Immutable;
using Nyeoglike.Unique.WorldGen.Tools.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nyeoglike.Unique.Globals;

namespace Nyeoglike.Unique.WorldGen.Tools {
    public partial class Carve {
        private static readonly FrozenSet<RoomType> MustCenterRoomTypes = 
            new(RoomType.EntryZone, RoomType.Closet);

        // build links
        public void BuildLinks() {
            var hallway = CreateRoom(RoomType.Hallway);

            foreach (var link in _links.Values) {
                if (link.Type == LinkType.Ignore) { continue; }

                var p = Predicates(link.Room1);
                var doorWorthy = new FrozenSet<DoorOption>(
                    from t in _tiles.Fwd[link.Room0]
                    from option in new[] {
                        new DoorOption(Label.Horiz, t, new V2(0, 1)),
                        new DoorOption(Label.Horiz, t, new V2(0, -1)),
                        new DoorOption(Label.Vert, t, new V2(1, 0)),
                        new DoorOption(Label.Vert, t, new V2(-1, 0)),
                    }
                    where !p.Claimed(option.Location)
                    select option
                );
                var doorWorthyVecs = new FrozenSet<V2>(from d in doorWorthy select d.Location);

                if (doorWorthy.Count == 0) { Veto(); }

                if (link.Type == LinkType.Counter || link.Type == LinkType.Door) {
                    Predicate<ID<Room>> mustCenter = (id) => MustCenterRoomTypes.Contains(_rooms[id].Type);
                    bool mustBeCentered = mustCenter(link.Room0) || mustCenter(link.Room1);

                    // TODO: This could be a utility function
                    DoorOption door;
                    if (mustBeCentered) {
                        var sortedSpots = (
                            from d in doorWorthy
                            orderby d.Location.X, d.Location.Y // all the Xs are the same, or all the Ys, so it doesn't matter
                            select d
                        ).ToArray();
                        door = sortedSpots[sortedSpots.Length / 2];
                    } 
                    else {
                        var bestSpot = (
                            from d in doorWorthy
                            orderby DoorScore(d) descending
                            select d
                        ).Max();
                        var maxScore = DoorScore(bestSpot);
                        var bestSpots = new FrozenSet<DoorOption>(
                            from d in doorWorthy
                            where DoorScore(d) == maxScore
                            select d
                        );
                        door = W.RNG.Choice(bestSpots);
                    }

                    if (link.Type == LinkType.Door) {
                        CarveTile(door.Location, hallway);
                    }
                    else { // Counter
                        CarveTile(door.Location, link.Room0);

                        foreach (var d in doorWorthyVecs) {
                            if (d == door.Location) { continue; }

                            CarveTile(d, link.Room0);
                            _hints.Fwd[d].Add(Hint.Counter);

                            foreach (var n in d.OrthoNeighbors) {
                                if (doorWorthyVecs.Contains(n)) { continue; }
                                _hints.Fwd[d].Add(Hint.Counterside);
                            }
                        }
                    }
                }
                else if (link.Type == LinkType.Complete) {
                    foreach (var i in doorWorthy) {
                        CarveTile(i.Location, link.Room0);
                    }
                } else {
                    throw new NotImplementedException($"unrecognized link type: {link.Type}");
                }
            }
        }

        private int DoorScore(DoorOption d) {
            if (d.Label == Label.Horiz && d.Location.X % _aligner.X == _aligner.Cx) {
                return 1;
            }
            if (d.Label == Label.Vert && d.Location.Y % _aligner.Y == _aligner.Cy) {
                return 1;
            }
            return 0;
        }

        private class DoorOption: IComparable<DoorOption>, IComparable {
            public Label Label;
            public V2 Location;
            public V2 FarLocation;  // TODO: Check all the tiles between FarLocation and Location, by direction instead

            public DoorOption(Label l, V2 v, V2 direction) {
                Label = l;
                Location = v + direction;
                FarLocation = v + direction + direction;
            }

            public int CompareTo(DoorOption other) {
                var c = Label.CompareTo(other.Label);
                if (c != 0) { return c; }
                c = Location.CompareTo(other.Location);
                if (c != 0) { return c; }
                return FarLocation.CompareTo(other.FarLocation);
            }

            public int CompareTo(object obj) => 
                ((IComparable<DoorOption>)this).CompareTo((DoorOption)obj);
        }

        private enum Label { Horiz, Vert, }
    }
}
