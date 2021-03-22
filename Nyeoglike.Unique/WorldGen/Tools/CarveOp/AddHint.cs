using Nyeoglike.Lib;
using Nyeoglike.Unique.WorldGen.Tools.Records;

namespace Nyeoglike.Unique.WorldGen.Tools.CarveOp {
    public struct AddHint: ICarveOp {
        public V2 Position;
        public Hint Hint;
    }
}
