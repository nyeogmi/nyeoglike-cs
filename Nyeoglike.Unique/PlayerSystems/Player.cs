using Nyeoglike.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.PlayerSystems {
    public class Player {
        public string Name => "Nyeogmi";

        // TODO: Move to a separate Pawn class?
        public V2 Cam { get; set; }
        public V2 Pos { get; set; }
    }
}
