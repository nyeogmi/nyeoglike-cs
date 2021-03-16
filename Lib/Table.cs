using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    class Table<T> {
        // TODO: Provide deleted-marking, but not actual deletion
        private List<T> Ts;
        
        public Table() {
            Ts = new List<T>();
        }

        public ID<T> Add(T t) {
            var ix = Ts.Count;
            Ts.Add(t);

            return new ID<T>(ix + 1);
        }

        public T this[ID<T> id] {
            get {
                var ix = CheckID(id);
                return Ts[ix];
            }
            set {
                var ix = CheckID(id);
                Ts[ix] = value;
            }
        }

        private int CheckID(ID<T> id) {
            if (!id.Initialized) {
                throw new ArgumentException($"ID must be initialized: {id}");
            }

            var ix = id.Index - 1;
            if (ix < 0) {
                throw new ArgumentException($"ID too low: {id}");
            }
            if (ix >= Ts.Count) {
                throw new ArgumentException($"ID too high: {id}");
            }
            return ix;
        }
    }

    struct ID<T>: IComparable<ID<T>> {
        public int Index { get; internal set; }

        public bool Initialized => Index != 0;

        internal ID(int index) {
            Index = index;
        }

        public int CompareTo(ID<T> other) => Index.CompareTo(other.Index);
    }
}
