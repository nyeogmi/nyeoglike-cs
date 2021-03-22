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
        private class _Predicates {
            public Predicate<V2> Claimed, ClaimedNotMe, ClaimedMe;
        }

        _Predicates Predicates(ID<Room> r) => new _Predicates {
            Claimed = (v) => _tiles.Rev[v].HasValue,
            ClaimedNotMe = (v) => {
                var owner = _tiles.Rev[v];
                return owner.HasValue && owner != r;
            },
            ClaimedMe = (v) => _tiles.Rev[v] == r
        };

        // TODO: Ident Rooms
    }
}
