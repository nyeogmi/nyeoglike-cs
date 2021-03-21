using Nyeoglike.Lib;
using Nyeoglike.Unique.NPCSystems;

namespace Nyeoglike.Unique.Events {
    public struct QuestStatus {
        public string Name;
        public string Description;
        public string OneLiner;
        public QuestOutcome Outcome;
        public ID<NPC>? Assigner;
        public bool IsChallenge;

        // TODO: The item the quest is about?
    }
}
