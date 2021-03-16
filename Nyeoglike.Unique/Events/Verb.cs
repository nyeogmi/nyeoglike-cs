using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Events {
    public enum Verb {
        Claim,
        Tick,
        AddFlag,
    }

    public static class VerbOps {
        public static bool QuestOnly(this Verb verb) {
            switch (verb) {
                case Verb.Claim: return true;
            }
            return false;
        }
    }
}
