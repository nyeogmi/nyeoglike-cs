using Nyeoglike.Lib;
using Nyeoglike.Unique.NPCSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Events {
    public abstract class Arg {
    }

    public class StringArg: Arg { 
        public string Value { get; private set; } 

        public StringArg(string value) {
            Value = value;
        }
    }

    public class EMArg: Arg { 
        public ID<EventMonitor> Value { get; private set; } 
        
        public EMArg(ID<EventMonitor> value) {
            Value = value;
        }
    }

    public class PushArg: Arg { 
        public ID<Push> Value { get; private set; }

        public PushArg(ID<Push> value) {
            Value = value;
        }
    }

    public class NPCArg: Arg { 
        public ID<NPC> Value { get; private set; }

        public NPCArg(ID<NPC> value) {
            Value = value;
        }
    }

    public class ClaimBoxArg: Arg { 
        public ClaimBox Value { get; private set; }

        public ClaimBoxArg(ClaimBox value) {
            Value = value;
        }
    }

    public class SceneFlagArg: Arg { 
        public SceneFlag Value { get; private set; }

        public SceneFlagArg(SceneFlag value) {
            Value = value;
        }
    }
}
