using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Events {
    public class Event {
        public Verb Verb { get; private set; }
        public Arg[] Args { get; private set; }  // TODO: Immutable array?

        public Event(Verb verb, params Arg[] args) {
            Verb = verb;
            Args = args;
        }
    }
}
