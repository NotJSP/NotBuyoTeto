using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotTetrin.Ingame.SinglePlay;

namespace NotTetrin.Constants {
    public static class PlayerPrefsKey {
        public static readonly string PlayerName = @"player_name";

        public static Dictionary<RankingType, string> HighScore = new Dictionary<RankingType, string>() {
            { RankingType.StackMode, @"stack_high_score" },
            { RankingType.MarathonMode, @"marathon_high_score" },
        };

        public static Dictionary<RankingType, string> ObjectId = new Dictionary<RankingType, string>() {
            { RankingType.StackMode, @"stack_object_id" },
            { RankingType.MarathonMode, @"marathon_object_id" },
        };
    }
}
