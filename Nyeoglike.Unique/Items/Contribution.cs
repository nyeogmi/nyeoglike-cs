using System;

namespace Nyeoglike.Unique.Items {
    public struct Contribution: IComparable<Contribution>, IComparable {
        public Resource Resource;
        public int N;

        public int CompareTo(Contribution other) {
            var c = Resource.CompareTo(other.Resource);
            if (c != 0) { return c; }
            return N.CompareTo(other.N);
        }

        public int CompareTo(object obj) {
            return ((IComparable<Contribution>)this).CompareTo((Contribution)obj);
        }
    }
}
