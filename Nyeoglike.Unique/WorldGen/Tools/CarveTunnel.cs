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
        public Snake Snake(ID<Room> room, Cardinal direction) => new Snake(this, room, direction);

        public ID<Room> TunnelEast(
            ID<Room> room,
            V2 size,
            RoomType type,
            int? minContact=null,
            bool useIgnore=false,
            Rule rule=Rule.RNG
        ) {
            var tiles = _tiles.Fwd[room];
            if (tiles.Count == 0) { Veto(); }
            var maxX = (from t in tiles select t.X).Max();
            var rhsTiles = from t in tiles where t.X == maxX select t;

            if (minContact == null) {
                minContact = useIgnore ? size.Y : 1;
            }

            return Tunnel(room, size, type, minContact.Value, useIgnore, rhsTiles.ToList(), new V2(1, 0), rule);
        }

        public ID<Room> TunnelSouth(
            ID<Room> room,
            V2 size,
            RoomType type,
            int? minContact=null,
            bool useIgnore=false,
            Rule rule=Rule.RNG
        ) {

            var tiles = _tiles.Fwd[room];
            if (tiles.Count == 0) { Veto(); }
            var maxY = (from t in tiles select t.Y).Max();
            var botTiles = from t in tiles where t.Y == maxY select t;

            if (minContact == null) {
                minContact = useIgnore ? size.X : 1;
            }

            return Tunnel(room, size, type, minContact.Value, useIgnore, botTiles.ToList(), new V2(0, 1), rule);
        }

        public ID<Room> TunnelWest(
            ID<Room> room,
            V2 size,
            RoomType type,
            int? minContact=null,
            bool useIgnore=false,
            Rule rule=Rule.RNG
        ) {
            var tiles = _tiles.Fwd[room];
            if (tiles.Count == 0) { Veto(); }
            var minX = (from t in tiles select t.X).Min();
            var lhsTiles = from t in tiles where t.X == minX select t - new V2(size.X - 1, 0);

            if (minContact == null) {
                minContact = useIgnore ? size.Y : 1;
            }

            return Tunnel(room, size, type, minContact.Value, useIgnore, lhsTiles.ToList(), new V2(-1, 0), rule);
        }

        public ID<Room> TunnelNorth(
            ID<Room> room,
            V2 size,
            RoomType type,
            int? minContact=null,
            bool useIgnore=false,
            Rule rule=Rule.RNG
        ) {
            var tiles = _tiles.Fwd[room];
            if (tiles.Count == 0) { Veto(); }
            var minY = (from t in tiles select t.Y).Min();
            var topTiles = from t in tiles where t.Y == minY select t - new V2(0, size.Y - 1);

            if (minContact == null) {
                minContact = useIgnore ? size.X : 1;
            }

            return Tunnel(room, size, type, minContact.Value, useIgnore, topTiles.ToList(), new V2(0, -1), rule);
        }

        private ID<Room> Tunnel(
            ID<Room> room,
            V2 size,
            RoomType type,
            int minContact,
            bool useIgnore,
            List<V2> tiles,
            V2 direction,
            Rule rule
        ) {
            var sites = new FrozenSet<R2>(
                from t in tiles
                let site = useIgnore
                    ? (t + direction).Sized(size)
                    : (t + direction * 2).Sized(size)
                where (
                    from t2 in site
                    where HasContact(room, site, t2, useIgnore ? 1 : 2, -direction)
                    select t2
                ).Count() >= minContact
                select site
            );

            if (sites.Count == 0) { Veto(); }

            return CarveRoom(
                ChooseTunnelSite(sites, rule),
                type,
                useIgnore ? new FrozenSet<ID<Room>>(room) : new FrozenSet<ID<Room>>()
            );
        }

        private R2 ChooseTunnelSite(FrozenSet<R2> sites, Rule rule) {
            if (rule == Rule.RNG) {
                return W.RNG.Choice(sites);
            }

            if (rule == Rule.Dense) {
                // TODO: Try to pick sites that minimize corners (aligned with top or bottom of the room)
                var scored = (
                    from site in sites
                    let score = (
                        from t in site
                        where HasContact(null, site, t, 2, null)
                        select t
                    ).Count()
                    select new { Site = site, Score = score }
                ).ToArray();
                var maxScore = (from sc in scored select sc.Score).Max();
                var maxedSites = new FrozenSet<R2>(
                    from sc in scored
                    where sc.Score == maxScore
                    select sc.Site
                );
                return W.RNG.Choice(maxedSites);
            }

            throw new NotImplementedException($"unknown rule: {rule}");
        }

        private bool HasContact(ID<Room>? room, R2 site, V2 t, int distance, V2? direction) {
            foreach (var dir in direction.HasValue ? new[] {direction.Value} : V2.Zero.OrthoNeighbors) {
                var present = _tiles.Rev[t + dir * distance];

                if (room.HasValue) {
                    if (present != room) { continue; }
                } 
                else {
                    if (present.HasValue) { continue; }
                }

                var blocked = (
                    from i in Enumerable.Range(0, distance - 1)
                    let t2 = t + dir * i
                    where site.Contains(t2) || _tiles.Rev[t2].HasValue 
                    select true
                ).Any();

                if (!blocked) {
                    return true;
                }
            }

            return false;
        }
    }
}
