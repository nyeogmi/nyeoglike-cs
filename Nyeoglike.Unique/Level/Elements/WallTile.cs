using Nyeoglike.Unique.DisplayData;
using static Nyeoglike.Unique.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.Level.Elements {
    public struct WallTile {
        public char Disp1;
        public char Disp2;
        public Color Fg;
        public Color Cap;
        public bool Flip;

        public static WallTile Default => new WallTile {
            Disp1 = '\xb0',
            Disp2 = '\xb0',
            Fg = Colors.WorldFG,
            Cap = Colors.WorldFG,
            Flip = false
        };

        public static WallTile GenerateWallpaper() {
            // also includes stuccos
            char disp1, disp2;
            bool flip = false;

            switch (W.RNG.Next(2)) {
                default:
                case 0: disp1 = disp2 = '\xb0'; break;
                case 1: disp1 = disp2 = '\xb1'; break;
            }
            return new WallTile {
                Disp1 = disp1,
                Disp2 = disp2,
                Fg = W.RNG.Choice(WallColors.All),
                Cap = W.RNG.Choice(WallColors.All),
                Flip = flip
            };
        }

        public static WallTile GeneratePaneling() {
            char disp1, disp2;
            bool flip = true;
            switch (W.RNG.Next(5)) {
                default:
                case 0: disp1 = disp2 = '\xdb'; flip = false; break;
                case 1: disp1 = disp2 = '\xdb'; break;
                case 2: disp1 = disp2 = '\xc4'; break;
                case 3: disp1 = disp2 = '\xcd'; break;
                case 4: disp1 = disp2 = '\xcb'; break;
            }
            return new WallTile {
                Disp1 = disp1,
                Disp2 = disp2,
                Fg = W.RNG.Choice(WallColors.Colorful),
                Cap = W.RNG.Choice(WallColors.All),
                Flip = flip
            };
        }

        public static WallTile GenerateTile() {
            return new WallTile {
                Disp1 = '\xb2',
                Disp2 = '\xb2',
                Fg = W.RNG.Choice(WallColors.Colorful),
                Cap = W.RNG.Choice(WallColors.All),
                Flip = false
            };
        }

        public static WallTile GenerateBathroomTile() {
            return new WallTile {
                Disp1 = '\xb2',
                Disp2 = '\xb2',
                Fg = W.RNG.Choice(WallColors.Banal),
                Cap = W.RNG.Choice(WallColors.Banal),
                Flip = false
            };
        }
    }
}
