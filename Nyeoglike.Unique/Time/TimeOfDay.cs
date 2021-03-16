using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Time {
    public enum TimeOfDay {
        Evening,
        Dusk,
        Midnight,
        Dawn,
        Morning,
        Afternoon,
    }

    public static class TimeOfDayOps {
        public static string Display(this TimeOfDay tod) => tod.ToString();
        public static bool SunOut(this TimeOfDay tod) {
            if (tod == TimeOfDay.Morning) { return true; }
            if (tod == TimeOfDay.Afternoon) { return true; }
            return false;
        }

        public static TimeOfDay Next(this TimeOfDay tod) {
            switch (tod) {
                case TimeOfDay.Evening:
                    return TimeOfDay.Dusk;
                case TimeOfDay.Dusk:
                    return TimeOfDay.Midnight;
                case TimeOfDay.Midnight:
                    return TimeOfDay.Dawn;
                case TimeOfDay.Dawn:
                    return TimeOfDay.Morning;
                case TimeOfDay.Morning:
                    return TimeOfDay.Afternoon;
                case TimeOfDay.Afternoon:
                default:
                    return TimeOfDay.Evening;
            }
        }
    }
}
