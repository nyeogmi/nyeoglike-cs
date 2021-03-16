using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Time {
    public class Clock {
        public bool Started { get; private set; }
        public int Tick { get; private set; }
        // TODO: Enum?
        public string Weekday => throw new ArgumentException("TODO");
        public TimeOfDay TimeOfDay { get; private set; }
        public int Night { get; private set; }

        public void Start() {
            Started = true;
        }

        public void AdvanceTick() {
            Tick++;
        }

        public void AdvanceTime() {
            Started = false;
            TimeOfDay = TimeOfDay.Next();

            if (TimeOfDay == TimeOfDay.Evening) {
                AdvanceNight();
            }
        }

        public void AdvanceNight() {
            Night++;
        }
    }
}
