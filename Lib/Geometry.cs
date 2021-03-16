using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib {
    struct V2 {
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
    }
    struct R2 {
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
    }
}
