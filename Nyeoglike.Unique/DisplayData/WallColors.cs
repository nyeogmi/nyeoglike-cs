using Nyeoglike.Lib.Immutable;

namespace Nyeoglike.Unique.DisplayData {
    public static class WallColors {
        public static Color
            Base = Colors.WorldFG,
            Yellow0 = Color.YellowGreen0,
            Green0 = Color.Green0,
            Red0 = Color.Red0,
            Sky0 = Color.Sky0,
            Fuchsia0 = Color.Fuchsia0
        ;

        public static readonly FrozenSet<Color> All = new(
            Base, Yellow0, Green0, Red0, Sky0, Fuchsia0
        );

        public static readonly FrozenSet<Color> Colorful = new(
            Yellow0, Green0, Red0, Sky0, Fuchsia0
        );

        public static readonly FrozenSet<Color> Banal = new(
            Base
        );
    }
}
