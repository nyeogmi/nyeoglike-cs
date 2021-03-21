namespace Nyeoglike.Unique.Events {
    public abstract class EventMonitor {
        public abstract Done Notify(Event evt); 
        public abstract QuestStatus? QuestStatus();
        public abstract object Key { get; }
    }
}
