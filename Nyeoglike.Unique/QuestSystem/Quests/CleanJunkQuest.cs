using Nyeoglike.Unique.Events;
using static Nyeoglike.Unique.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyeoglike.Lib;

namespace Nyeoglike.Unique.QuestSystem.Quests {
    class CleanJunkQuest : EventMonitor {
        private ID<EventMonitor> _id;
        private int _initialJunk;
        private int _junkLeft;
        private bool _completed = false;

        public CleanJunkQuest(ID<EventMonitor> id, int junkLeft) {
            _id = id;
            _initialJunk = _junkLeft = junkLeft;
        }

        public override Done Notify(Event evt) {
            if (evt.Verb == Verb.Tick) {
                _junkLeft = W.Level.Items.JunkLeft;

                if (_junkLeft == 0) {
                    _completed = true;
                }
            }

            return Done.NotDone;
        }

        public override QuestStatus? QuestStatus() {
            if (!_completed) {
                return new QuestStatus {
                    Name = "Clean up junk",
                    Description = "This place is full of junk. Clean it!",
                    OneLiner = $"Clean junk ({_junkLeft}/{_initialJunk})",
                    Outcome = QuestOutcome.InProgress,
                    Assigner = null,
                    IsChallenge = true,
                };
            } 
            else {
                return new QuestStatus {
                    Name = "Clean up junk",
                    Description = "You cleaned the junk!",
                    OneLiner = "Junk cleaned!",
                    Outcome = QuestOutcome.Succeeded,
                    Assigner = null,
                    IsChallenge = true,
                };
            }
        }

        public override object Key => typeof(CleanJunkQuest);
    }
}
