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
        public void ExpandDensely(ID<Room> r) {
            var p = Predicates(r);

            bool changeMade = true;
            while (changeMade) {
                var toAdd = new SortedSet<V2>();
                foreach (var d in V2.Zero.OrthoNeighbors) {
                    foreach (var t in _tiles.Fwd[r]) {
                        var t1 = p.Claimed(t + d);
                        if (t1) { continue; }

                        foreach (var n in (t + d).Neighbors) {
                            if (p.ClaimedNotMe(n)) { continue; }
                        }

                        if(p.ClaimedMe(t + d + d)) {
                            toAdd.Add(t + d);
                            continue;
                        }

                        var t2 = p.Claimed(t + d + d);
                        if (t2) { continue; }

                        var t3 = p.Claimed(t + d + d + d);
                        var t4 = p.Claimed(t + d + d + d + d);
                        var t5 = p.Claimed(t + d + d + d + d + d);

                        if (
                            (t5 && !(t4 || t3)) || 
                            (t4 && !(t3)) || 
                            (t3)
                        ) {
                            toAdd.Add(t + d);
                        }
                    }
                }

                changeMade = toAdd.Count > 0;
                foreach (var t in toAdd) {
                    CarveTile(t, r);
                }
            }
        }

        public void Erode(ID<Room> r, int iterations) {
            var p = Predicates(r);

            // TODO: Faster way to do this? I probably don't care
            var directions = V2.Zero.OrthoNeighbors.ToList();

            for (var i = 0; i < iterations; i++) {
                var toRemove = new SortedSet<V2>();

                for (var di = 0; di < directions.Count; di++) {
                    var d1 = directions[di];
                    var d2 = directions[di + 1 % directions.Count];

                    foreach (var t in _tiles.Fwd[r]) {
                        var tu1 = p.Claimed(t + d1);
                        var tu2 = p.Claimed(t + d1 + d1);
                        var tl1 = p.Claimed(t + d2);
                        var tl2 = p.Claimed(t + d2 + d2);

                        if (!(tu1 || tu2 || tl1 || tl2)) {
                            toRemove.Add(t);
                        }
                    }
                }

                foreach (var t in toRemove) {
                    CarveTile(t, null);
                }
            }
        }

        public void Erode1TileWonk(ID<Room> r) {
            // removes one-tile bacon strips
            var p = Predicates(r);
            var toRemove = new SortedSet<V2>();

            foreach (var d in new[]{new V2(-1, 0), new V2(0, -1)}) {
                foreach (var t in _tiles.Fwd[r]) {
                    if(!(p.Claimed(t + d) || p.Claimed(t - d))) {
                        toRemove.Add(t);
                    }
                }
            }

            foreach (var t in toRemove) {
                CarveTile(t, null);
            }
        }


        // TODO: continue from ident_rooms down in another file
    }
}
