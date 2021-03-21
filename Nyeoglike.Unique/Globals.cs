using Nyeoglike.Lib.FS.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique {
    public static class Globals {
        public static Store<Permanent> S { get; private set; }  // must be populated before the world is
        public static World W { get; private set; }
    }
}
