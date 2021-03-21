using Nyeoglike.Lib;
using Nyeoglike.Unique.Items;
using Nyeoglike.Unique.NPCSystems;
using Nyeoglike.Unique.PlayerSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nyeoglike.Unique.Globals;

namespace Nyeoglike.Unique.Events {
    public class EventMonitors {
        public DropTable<EventMonitor> Active;
        private HashSet<object> _activeKeys = new();  // TODO: string?

        private SortedDictionary<ID<EventMonitor>, QuestStatus> _mostRecentStatus = new();
        private List<ID<EventMonitor>> _acceptedQuests = new();
        private List<QuestStatus> _failedQuests = new();
        private List<QuestStatus> _succeededQuests = new();

        public EventMonitors() { }

        public ID<EventMonitor>? Add(
            Func<ID<EventMonitor>, EventMonitor> constructor
        ) {
            var id = Active.Add(constructor);
            var em = Active.ForceGetValue(id);

            if (_activeKeys.Contains(em.Key)) {
                // this can't be an active EM if it's a duplicate
                Active.Remove(id);
                return null;
            }
            _activeKeys.Add(em.Key);

            var mquestStatus = em.QuestStatus();
            if (!mquestStatus.HasValue) {
                // done!!!
                return id;
            }

            // auto accept the quest
            var questStatus = mquestStatus.Value;
            _acceptedQuests.Add(id);
            _mostRecentStatus[id] = questStatus;

            if (questStatus.IsChallenge) {
                // always accept challenges w/o a negotiation
                return id;
            }

            // give the player a chance to reject the quest
            W.Pushes.Send(id, PushReason.AnnounceQuest, questStatus.Assigner);
            return id;
        }

        public QuestStatus? MostRecentStatus(ID<EventMonitor> quest) {
            if (_mostRecentStatus.TryGetValue(quest, out QuestStatus qs)) {
                return qs;
            }
            return null;
        }

        public void AcceptQuest(ID<EventMonitor> quest) {
            if (Active.ContainsKey(quest) && !_acceptedQuests.Contains(quest)) {
                _acceptedQuests.Append(quest);
            }
        }

        public void IgnoreQuest(ID<EventMonitor> quest) {
            if (_acceptedQuests.Contains(quest)) {
                _acceptedQuests.Remove(quest);
            }

            // TODO: Remove items claimed by the quest?
        }

        // TODO: Accessor for accepted quests?

        public ID<NPC>? WhoWants(Item item) {
            var claimBox = new ClaimBox(item, true);
            try {
                Notify(new Event(Verb.Claim, new ClaimBoxArg(claimBox)));
                return null;
            } 
            catch (HypotheticalClaimException hce) {
                return Active[hce.Claimant].QuestStatus()?.Assigner;
            }
        }

        public void Notify(Event evt) {
            var doneEms = new SortedSet<ID<EventMonitor>>();

            // TODO: Iterate in reverse precedence order of quests
            // (based on how much the player likes the NPC)
            // Then do non-quests last
            foreach (var kv in Active) {
                var id = kv.Key;
                var em = kv.Value;

                var questStatus = em.QuestStatus();

                bool shouldNotify;
                if (evt.Verb.QuestOnly()) {
                    shouldNotify = questStatus?.Outcome == QuestOutcome.InProgress;

                    if (evt.Verb == Verb.Claim && !_acceptedQuests.Contains(id)) {
                        shouldNotify = false;
                    }
                }
                else {
                    shouldNotify = true;
                }

                if (shouldNotify) {
                    if (em.Notify(evt) == Done.Done) {
                        doneEms.Add(id);
                    }
                    questStatus = em.QuestStatus();
                }

                if (questStatus.HasValue) {
                    var qs = questStatus.Value;
                    if (qs.Outcome == QuestOutcome.Failed) {
                        doneEms.Add(id);
                        _failedQuests.Add(qs);
                        SendFinalizeQuest(id, qs);
                    }

                    if (qs.Outcome == QuestOutcome.Succeeded) {
                        doneEms.Add(id);
                        _succeededQuests.Add(qs);
                        SendFinalizeQuest(id, qs);
                    }

                    _mostRecentStatus[id] = qs;
                }

                if (
                    evt.Verb == Verb.Claim && (
                        from arg in evt.Args
                        where arg is ClaimBoxArg
                        let v = ((ClaimBoxArg) arg).Value
                        select v 
                    ).All(i => i.Taken)
                ) {
                    // We're done with this evt
                    break;
                }
            }

            foreach (var d in doneEms) {
                _activeKeys.Remove(Active.ForceGetValue(d).Key);
                W.Pushes.RemoveFor(d, PushReason.AnnounceQuest);
                Active.Remove(d);
            }
        }

        public void SendFinalizeQuest(ID<EventMonitor> id, QuestStatus questStatus) {
            if (questStatus.IsChallenge) {
                // Do nothing. The Challenges object handles finalization
            }
            else if (_acceptedQuests.Contains(id) || questStatus.Outcome == QuestOutcome.Succeeded) {
                // Show the user a success even if they didn't accept it
                W.Pushes.Send(id, PushReason.FinalizeQuest, questStatus.Assigner);
            }
            else {
                // Don't wait for the user
                FinalizeQuest(id);
            }
        }

        public void FinalizeQuest(ID<EventMonitor> id) {
            if (_acceptedQuests.Contains(id)) {
                _acceptedQuests.Remove(id);
            }
        }
    }
}
