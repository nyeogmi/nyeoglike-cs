using Nyeoglike.Lib;
using Nyeoglike.Lib.FS.Hierarchy;
using Nyeoglike.Lib.Relations;
using Nyeoglike.Unique.WorldGen.Tools.CarveOp;
using Nyeoglike.Unique.WorldGen.Tools.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Nyeoglike.Unique.Globals;

namespace Nyeoglike.Unique.WorldGen.Tools {
    public partial class Carve {
        private Aligner _aligner;

        private DropTable<Room> _rooms;
        private OneToMany<ID<Room>, V2> _tiles;
        private DropTable<Link> _links;
        private OneToMany<V2, Hint> _hints;

        private List<ICarveOp> _operationLog;

        public Carve(Aligner aligner) {
            _aligner = aligner;

            _rooms = new(Node.Free);
            _links = new(Node.Free);
            _hints = new(Node.Free);

            _operationLog = new();
        }

        public InteriorDesigner ToInterior() => new InteriorDesigner(_rooms, _tiles, _hints);

        public void PermuteAtRandom() {
            var tiles = new OneToMany<ID<Room>, V2>(Node.Free);
            var hints = new OneToMany<V2, Hint>(Node.Free);

            var mulX = W.RNG.Choice(-1, 1);
            var mulY = W.RNG.Choice(-1, 1);
            var swap = W.RNG.Choice(false, true);

            var mul = new V2(mulX, mulY);

            Func<V2, V2> permute = (v) => {
                v *= mul;
                if (swap) { v = new V2(v.Y, v.X); }
                return v;
            };

            foreach (var kv in _tiles.Fwd) {
                tiles.Add(kv.Key, permute(kv.Value));
            }

            foreach (var kv in _hints.Fwd) {
                hints.Add(permute(kv.Key), kv.Value);
            }

            _tiles = tiles;
            _hints = hints;
        }

        public void VetoPoint(Action<VetoBox> body) {
            var point = _operationLog.Count;
            var box = new VetoBox();

            try {
                body(box);
                box.Vetoed = false;
            }
            finally {
                while (_operationLog.Count > point) {
                    var op = _operationLog.Count - 1;
                    this.UndoOp(_operationLog[op]);
                    _operationLog.RemoveAt(op);
                }
                box.Vetoed = true;
            }
        }

        public void Veto() => throw new Veto();
    }
}