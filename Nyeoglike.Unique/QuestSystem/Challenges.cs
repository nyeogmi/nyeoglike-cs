using static Nyeoglike.Unique.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nyeoglike.Lib;
using Nyeoglike.Unique.Events;
using Nyeoglike.Unique.QuestSystem.Quests;

namespace Nyeoglike.Unique.QuestSystem {
    public class Challenges {
        private ID<EventMonitor>? _cleanJunkChallenge;

        public Challenges() { }

        public void Scan() {
            // Make sure existing quests are removed
            Descan();

            if (W.Level.Items.JunkLeft > 0) {
                _cleanJunkChallenge = W.EventMonitors.Add(
                    handle => new CleanJunkQuest(handle, W.Level.Items.JunkLeft)
                );
            }
        }

        public void Descan() {
            if (_cleanJunkChallenge != null) {
                W.EventMonitors.FinalizeQuest(_cleanJunkChallenge.Value);
                _cleanJunkChallenge = null;
            }
        }
    }
}
