using Nyeoglike.Lib;
using Nyeoglike.Unique.NPCSystems.Capitalism;
using static Nyeoglike.Unique.Globals;

namespace Nyeoglike.Unique.NPCSystems.Scheduling {

    public struct ScheduleItem {
        public static ScheduleItem HomeFun => new ScheduleItem {
            Type = ScheduleItemType.HomeFun,
            Text = "home, not sleepy",
            FollowEnterprise = null,
            FollowNPC = null,
        };
        public static ScheduleItem HomeSleep => new ScheduleItem {
            Type = ScheduleItemType.HomeSleep,
            Text = "home to sleep",
            FollowEnterprise = null,
            FollowNPC = null,
        };
        public static ScheduleItem HostSleepover => new ScheduleItem {
            Type = ScheduleItemType.HostSleepover,
            Text = "host a sleepover",
            FollowEnterprise = null,
            FollowNPC = null,
        };
        public static ScheduleItem GoToSleepover(ID<NPC> id) => new ScheduleItem {
            Type = ScheduleItemType.HostSleepover,
            Text = $"sleepover with {W.NPCs.Table[id].Name}",
            FollowEnterprise = null,
            FollowNPC = null,
        };
        public static ScheduleItem GoToWork(ID<Enterprise> id) => new ScheduleItem {
            Type = ScheduleItemType.HostSleepover,
            Text = $"work at with {W.Enterprises.Table[id].Name}",
            FollowEnterprise = null,
            FollowNPC = null,
        };

        // NOTE: We're getting rid of the old DestinationRule system.
        // It's MyHousehold unless a Follow exists. In that case, use that
        public ScheduleItemType Type;
        public string Text;
        public ID<Enterprise>? FollowEnterprise;
        public ID<NPC>? FollowNPC;
    }
}
