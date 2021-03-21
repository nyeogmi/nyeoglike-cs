using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib.FS.Hierarchy {
    public interface AnyNode {
        AnyNode GenericSub(string path);
        void Bind(IPrimitive primitive);
    }
}
