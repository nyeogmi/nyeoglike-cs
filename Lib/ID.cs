using System;

namespace Nyeoglike.Lib {
    public struct ID<T>: IComparable<ID<T>>, IComparable {
        public int Index { get; internal set; }

        public bool Initialized => Index != 0;

        internal ID(int index) {
            Index = index;
        }

        public int CompareTo(ID<T> other) => Index.CompareTo(other.Index);
        public int CompareTo(object other) => ((IComparable<ID<T>>)this).CompareTo((ID<T>)other);

        public static bool operator ==(ID<T> one, ID<T> two) => one.Index == two.Index;
        public static bool operator !=(ID<T> one, ID<T> two) => one.Index != two.Index;

        public override bool Equals(object obj) {
            if (!(obj is ID<T>)) { return false;  }
            return this == (ID<T>)obj;
        }

        public override int GetHashCode() => Index.GetHashCode();

        public static bool operator <(ID<T> left, ID<T> right) {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(ID<T> left, ID<T> right) {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(ID<T> left, ID<T> right) {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(ID<T> left, ID<T> right) {
            return left.CompareTo(right) >= 0;
        }
    }
}
