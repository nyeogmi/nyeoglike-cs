using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.WorldGen.Tools.Records {
    public enum Cardinal {
        North = 0,
        East = 1,
        South = 2,
        West = 3,
    }

    public static class CardinalOps {
        public static Cardinal Right(this Cardinal cardinal) =>
            (Cardinal)((((int)cardinal) + 1) % 4);

        public static Cardinal Left(this Cardinal cardinal) =>
            (Cardinal)((((int)cardinal) + 3) % 4);
    }
}
