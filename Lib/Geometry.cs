using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Lib {
    public struct V2: IComparable<V2>, IComparable {
        public int X, Y;

        public V2(int x, int y) {
            X = x;
            Y = y;
        }

        public static V2 Zero => new V2 { X = 0, Y = 0 };

        public bool IsUnsigned => X >= 0 && Y >= 0;

        public R2 To(V2 other) => new R2(X, Y, other.X, other.Y);
        public R2 Sized(V2 other) => new R2(X, Y, X + other.X, Y + other.Y);

        public static V2 operator +(V2 self, V2 other) => new V2(self.X + other.X, self.Y + other.Y);
        public static V2 operator -(V2 self, V2 other) => new V2(self.X - other.X, self.Y - other.Y);
        public static V2 operator *(V2 self, V2 other) => new V2(self.X * other.X, self.Y * other.Y);
        public static V2 operator *(V2 self, int other) => new V2(self.X * other, self.Y * other);
        public static V2 operator *(int other, V2 self) => self * other;
        public static V2 operator -(V2 self) => new V2(-self.X, -self.Y);

        public int Manhattan(V2 other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

        public IEnumerable<V2> OrthoNeighbors {
            get {
                yield return new V2(X - 1, Y);
                yield return new V2(X, Y - 1);
                yield return new V2(X + 1, Y);
                yield return new V2(X, Y + 1);
            }
        }

        public IEnumerable<V2> Neighbors {
            get {
                for (var y = -1; y < 2; y++) {
                    for (var x = -1; x < 2; x++) {
                        if (x == 0 && y == 0) { continue; }
                        yield return new V2(X + x, Y + y);
                    }
                }
            }
        }

        public static bool operator ==(V2 one, V2 two) => one.X == two.X && one.Y == two.Y;
        public static bool operator !=(V2 one, V2 two) => !(one == two);

        public override bool Equals(object obj) {
            if (!(obj is V2)) { return false; }
            return this == (V2)obj;
        }

        public override int GetHashCode() {
            int xhc = X.GetHashCode();
            int yhc = Y.GetHashCode();
            return (xhc << 16) ^ yhc ^ (xhc >> 16);
        }

        public int CompareTo(V2 other) {
            var c1 = X.CompareTo(other.X);
            if (c1 != 0) { return c1; }
            return Y.CompareTo(other.Y);
        }

        public int CompareTo(object obj) => ((IComparable<V2>)this).CompareTo((V2)obj);
    }

    public struct R2: IEnumerable<V2>, IComparable<R2>, IComparable {
        public V2 Top, Size;

        public R2(int x0, int y0, int x1, int y1) {
            var xTop = Math.Min(x0, x1);
            var yTop = Math.Min(y0, y1);
            var xBot = Math.Max(x0, x1);
            var yBot = Math.Max(y0, y1);

            Top = new V2(xTop, yTop);
            Size = new V2(xBot, yBot);
        }

        public R2(V2 top, V2 size) {
            // TODO: Assert valid?
            Top = top;
            Size = size;
        }

        public IEnumerable<V2> InclusiveCorners {
            get {
                var bot = BotInclusive;
                yield return new V2(Top.X, Top.Y);
                yield return new V2(bot.X, Top.Y);
                yield return new V2(Top.X, bot.Y);
                yield return new V2(bot.X, bot.Y);
            }
        }

        public V2 BotInclusive {
            get {
                if (Size.X <= 0 || Size.Y <= 0) {
                    throw new InvalidOperationException("rectangle has zero size; no bottom corner");
                }
                return Top + Size - new V2(1, 1);
            }
        }

        public V2 BotExclusive => Top + Size;

        // TODO: Assert amt > 0?
        public R2 Expand(V2 amt) => new R2(Top - amt, Size + amt * 2);

        public static bool operator ==(R2 one, R2 two) => one.Top == two.Top && one.Size == two.Size;
        public static bool operator !=(R2 one, R2 two) => !(one == two);

        public override bool Equals(object obj) {
            if (!(obj is R2)) { return false;  }
            return this == (R2)obj;
        }

        public override int GetHashCode() {
            int xhc = Top.GetHashCode();
            int yhc = Size.GetHashCode();
            return (xhc << 8) ^ yhc ^ (xhc >> 24);
        }

        public int CompareTo(R2 other) {
            var c1 = Top.CompareTo(other.Top);
            if (c1 != 0) { return c1; }
            return Size.CompareTo(other.Size);
        }

        public int CompareTo(object obj) => ((IComparable<R2>)this).CompareTo((R2)obj);

        public bool Contains(V2 v) =>
            Top.X <= v.X && v.X < Top.X + Size.X && 
            Top.Y <= v.Y && v.Y < Top.Y + Size.Y;

        public IEnumerator<V2> GetEnumerator() {
            for (var y = 0; y < Size.Y; y++) {
                for (var x = 0; x < Size.X; x++) {
                    yield return Top + new V2(x, y);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
