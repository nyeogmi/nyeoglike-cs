using Nyeoglike.Lib.Immutable;
using System;

namespace Nyeoglike.Unique {
    public class RNG {
        private Random _random;

        public RNG() {
            _random = new Random();
        }

        public T Choice<T>(FrozenSet<T> ts) where T: IComparable => ts.Choice(_random);
        public T Choice<T>(params T[] ts) => ts[_random.Next(ts.Length)];
        public int Next(int max) => _random.Next(max);
    }
}