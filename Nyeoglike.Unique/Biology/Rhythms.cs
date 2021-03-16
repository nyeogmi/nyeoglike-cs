using Nyeoglike.Lib;
using Nyeoglike.Unique.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Biology {
    /*
# Notes:
# Each "_quantity" is 4 hours worth.
# Most creatures eat 2 meals/day, so each meal is worth 12 hours; 3
# Likewise, most creatures sleep 8 hours a day, so each 4 hour nap is worth 12 hours; 3.
# Some creatures sleep 12 hours a day, so each 4 hour nap is worth 8 hours for them; 2.  (Probably daytime naps work like this)
#  (Note that 4 hours of that is instantly depleted at the end of the nap.)
# The cap is 10: in other words, a maximally fed creature will be maximally hungry or sleepy in 40 hours

# The spectrum:
#  >= 10 - Capped. Doing it more causes no additional good
#  >=  8 - Satisfied. Users will deviate from their usual schedule if higher than this
#  >=  6 - Scheduled. Users will satisfy the need according to their usual schedule if higher than this.
#  >=  3 - Desperate. Users will deviate from unimportant items of their usual schedule if higher than this.
#  >=  0 - Emergency. The user can't need it any more than this
     */
    public enum Spectrum {
        Capped = 0, 
        Satisfied = 1, 
        Scheduled = 2, 
        Desperate = 3, 
        Emergency = 4
    }

    class Rhythm {
        private Dictionary<ID<NPC>, int> _quantity;

        public Rhythm() {
            _quantity = new Dictionary<ID<NPC>, int>();
        }

        public void AdvanceTime() {
            foreach (var npc in _quantity.Keys) {
                _quantity[npc] = Math.Max(0, _quantity[npc] - 1);
            }
        }

        public void Feed(ID<NPC> npc, int quantity) {
            _quantity[npc] = Math.Min(10, _quantity.GetValueOrDefault(npc, 0) + quantity);
        }

        public Spectrum Get(ID<NPC> npc) {
            var value = _quantity.GetValueOrDefault(npc, 0);
            return
                value >= 10 ? Spectrum.Capped :
                value >= 8 ? Spectrum.Satisfied :
                value >= 6 ? Spectrum.Scheduled :
                value >= 3 ? Spectrum.Desperate :
                Spectrum.Emergency;
        }
    }

    public class Rhythms {
        private Rhythm _sleepiness = new Rhythm();
        private Rhythm _hunger = new Rhythm();

        public Rhythms() { }

        public void AdvanceTime() {
            _sleepiness.AdvanceTime();
            _hunger.AdvanceTime();
        }

        public void Notify(Event evt) {
            throw new ArgumentException("TODO: Scene flag stuff");
        }
        
        public void Sleep(ID<NPC> npc, int quantity) => _sleepiness.Feed(npc, quantity);
        public void Feed(ID<NPC> npc, int quantity) => _hunger.Feed(npc, quantity);
        public Spectrum GetSleepiness(ID<NPC> npc) => _sleepiness.Get(npc);
        public Spectrum GetHunger(ID<NPC> npc) => _hunger.Get(npc);

        public bool CanSleep(ID<NPC> npc) =>
            // TODO: Check the NPC schedule, don't say "yes" unless it's sleeping time
            (int) GetSleepiness(npc) >= (int) Spectrum.Scheduled;

        public bool IsSleepy(ID<NPC> npc) =>
            (int) GetSleepiness(npc) >= (int) Spectrum.Desperate;

        public bool IsVerySleepy(ID<NPC> npc) =>
            (int) GetSleepiness(npc) >= (int) Spectrum.Emergency;

        public bool CanEat(ID<NPC> npc) =>
            // TODO: Check the NPC schedule, don't say "yes" unless it's sleeping time
            (int) GetHunger(npc) >= (int) Spectrum.Scheduled;

        public bool IsHungry(ID<NPC> npc) =>
            (int) GetHunger(npc) >= (int) Spectrum.Desperate;

        public bool IsVeryHungry(ID<NPC> npc) =>
            (int) GetHunger(npc) >= (int) Spectrum.Emergency;
    }
}
