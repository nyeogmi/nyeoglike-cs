using Nyeoglike.Unique.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.WorldMap {
    public delegate UnloadedLevel Generator();

    public class Realtor {
        private List<Lot> _lots;
        private Generator _generator;


        private const int NLots = 20;
        private const int MaxTimesUnsold = 5;
        private const int TriesPerLot = 5;

        public Realtor(Generator generator) {
            _generator = generator;
        }

        public UnloadedLevel Generate(Demand demand) {
            Restock();

            var bestLotIx = (
                from l in Enumerable.Range(0, _lots.Count)
                orderby demand.Score(_lots[l])
                select l
            ).First();
            var lot = _lots[bestLotIx];
            _lots.RemoveAt(bestLotIx);

            foreach (var l in Enumerable.Range(0, _lots.Count)) {
                var existing = _lots[l];
                existing.TimesUnsold += 1;
                _lots[l] = existing;
            }

            return lot.Level;
        }

        private void Restock() {
            _lots = (
                from lot in _lots
                where lot.TimesUnsold <= MaxTimesUnsold
                select lot
            ).ToList();


            var lotsNeeded = NLots - _lots.Count;
            var maxTries = lotsNeeded * TriesPerLot;

            var tally = 0;
            for (var t = 0; t < maxTries; t++) {
                if (_lots.Count > NLots) {
                    return;
                }

                try {
                    var level = _generator();
                    _lots.Add(new Lot {
                        Level = level,
                        TimesUnsold = tally
                    });
                    tally = (tally + 1) % MaxTimesUnsold;
                } 
                catch (Veto) {
                    continue;
                }
            }

            throw new InvalidOperationException($"couldn't generate {lotsNeeded} lots in {maxTries} tries");
        }
    }
}
