using Nyeoglike.Lib;
using Nyeoglike.Unique.Events;
using static Nyeoglike.Unique.Globals;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique {
    public class Pushes {
        public DropTable<Push> Active = new();

        public Pushes() { }

        public ID<Push> Send(
            string message,
            PushReason reason,
            ID<NPC>? source
        ) => Send(message, null, reason, source);

        public ID<Push> Send(
            ID<EventMonitor> message,
            PushReason reason,
            ID<NPC>? source
        ) => Send(null, message, reason, source);

        public ID<Push> Send(
            string stringMessage,
            ID<EventMonitor>? emMessage,
            PushReason reason,
            ID<NPC>? source
        ) {
            return Active.Add(id => new Push {
                ID = id,
                StringMessage = stringMessage,
                EMMessage = emMessage,
                Reason = reason,
                Source = source,
            });
        }

        public void Acknowledge(ID<Push> id, bool yes) {
            if (!Active.Remove(id, out Push push)) {
                return;
            }

            if (push.EMMessage.HasValue) {
                // quest notification
                var quest = push.EMMessage.Value;

                if (push.Reason == PushReason.AnnounceQuest) {
                    if (yes) {
                        W.EventMonitors.AcceptQuest(quest);
                    }
                    else {
                        W.EventMonitors.IgnoreQuest(quest);
                    }
                }
                else if (push.Reason == PushReason.FinalizeQuest) {
                    W.EventMonitors.FinalizeQuest(quest);
                }
            }
        }

        // TODO: Order?
        public IEnumerable<Push> ActivePushes() => Active.Values;

        // TODO: Accessor for the last one?

        public void RemoveFor(ID<EventMonitor> quest, PushReason? reason) {
            var ixs =
                from kv in Active
                let push = kv.Value
                where push.EMMessage.HasValue && push.EMMessage.Value == quest
                where reason.HasValue ? push.Reason == reason : true
                select kv.Key;

            foreach (var ix in ixs.ToList()) {
                Active.Remove(ix);
            }
        }
    }
}
