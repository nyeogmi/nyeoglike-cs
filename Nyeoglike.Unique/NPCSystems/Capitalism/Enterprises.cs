using Nyeoglike.Lib;
using Nyeoglike.Lib.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.NPCSystems.Capitalism {
    public class Enterprises {
        public Table<Enterprise> Table;
        public Table<Shift> ShiftTable;
        public OneToMany<ID<Enterprise>, ID<Shift>> Shifts;
    }
}
