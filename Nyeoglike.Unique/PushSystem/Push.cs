using Nyeoglike.Lib;
using Nyeoglike.Unique.Events;
using System;

namespace Nyeoglike.Unique {
    public struct Push { 
        public ID<Push> ID;

        public string StringMessage;
        public ID<EventMonitor>? EMMessage;
        public string Message => throw new NotImplementedException("TODO");

        public PushReason Reason;
        public ID<NPC>? Source;
    }
}
