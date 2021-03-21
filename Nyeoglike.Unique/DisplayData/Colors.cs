using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyeoglike.Unique.DisplayData {
    public static class Colors {
        public const Color 
            TermBG = Color.Black0,
            TermFG = Color.White0,

            TermFGBold = Color.White1,

            TermHighlightBG = Color.Fuchsia0,
            TermHighlightBGInactive = Color.Grey1,

            FadeBG = Color.Black1,
            FadeFG = Color.Grey1,

            WorldBG = Color.Black0,
            WorldFG = Color.White0,

            WorldUnseenBG = Color.Black1,
            WorldUnseenFG = Color.Grey1,

            ResourceGeneric = Color.Grey1,  // TODO: Bad choice?

            Player = Color.White1,
            NPCNoInterest = Color.YellowGreen0,
            NPCFriend = Color.YellowGreen1,
            NPCLove = Color.Fuchsia1,
            MSGSystem = Color.Grey1,

            QuestSucceeded = Color.Fuchsia1,
            QuestFailed = Color.Red1
        ;
    }
}
