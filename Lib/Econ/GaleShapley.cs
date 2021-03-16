using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.Econ {
    // null if this A would rather have no one than consider this O
    public delegate int? PrefLevel<A, O>(A a, O o); 

    public class GaleShapley<X, Y> {
        private PrefLevel<X, Y> _prefXY; 
        private PrefLevel<Y, X> _prefYX;

        public struct Pair {
            int X, Y;

            public Pair(int x, int y) {
                X = x; 
                Y = y;
            }
        }

        // the index of the matched Y for each X
        public int?[] Match(X[] xs, Y[] ys) {
            var xInvites = (
                from x in Enumerable.Range(0, xs.Length)
                select (
                    from y in Enumerable.Range(0, ys.Length)
                    let pref = _prefXY(xs[x], ys[y])
                    where pref.HasValue
                    orderby pref.Value
                    select y
                ).ToList()
            ).ToList();

            var xMatched = new HashSet<int>();
            var yMailbox = new int?[ys.Length];

            Func<int, int, int?> invite = (int newX, int y) => {
                // return discarded item

                var oldX = yMailbox[y];
                var mpref = _prefYX(ys[y], xs[newX]);
                if (!mpref.HasValue) { return newX; }
                var newPref = mpref.Value;

                int useX;
                int? discardX;
                if (!oldX.HasValue) {
                    useX = newX;
                    discardX = null;
                }
                else {
                    var oldPref = _prefYX(ys[y], xs[oldX.Value]);

                    if (oldPref > newPref) {
                        useX = oldX.Value;
                        discardX = newX;
                    }
                    else {
                        useX = newX;
                        discardX = oldX.Value;
                    }
                }

                yMailbox[y] = useX;
                return discardX;
            };

            while (true) {
                var didWork = false;

                foreach (var x in Enumerable.Range(0, xs.Length)) {
                    var xinv = xInvites[x];
                    if (xinv.Count == 0) { continue; }
                    if (xMatched.Contains(x)) { continue; }

                    didWork = true;
                    var y = xinv[xinv.Count - 1];
                    xinv.RemoveAt(xinv.Count - 1);
                    var discarded = invite(x, y);

                    xMatched.Add(x);
                    if (discarded.HasValue) {
                        xMatched.Remove(discarded.Value);
                    }
                }

                if (!didWork) {
                    break;
                }
            }

            // final matching
            var xMatch = new int?[xs.Length];
            foreach (var y in Enumerable.Range(0, ys.Length)) {
                var mx = yMailbox[y];

                if (mx.HasValue) {
                    xMatch[mx.Value] = y;
                }
            }
            return xMatch;
        }
    }
}
