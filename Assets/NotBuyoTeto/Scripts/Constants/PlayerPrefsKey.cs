using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotBuyoTeto.Ingame.SinglePlay;

namespace NotBuyoTeto.Constants {
    public static class PlayerPrefsKey {
        public static readonly string PlayerId = @"player_id";
        public static readonly string PlayerName = @"player_name";

        public static Dictionary<RankingType, string> HighScore = new Dictionary<RankingType, string>() {
            { RankingType.MarathonMode, @"marathon_high_score" },
            { RankingType.TokotonMode, @"tokoton_high_score" },
        };

        public static Dictionary<RankingType, string> ObjectId = new Dictionary<RankingType, string>() {
            { RankingType.MarathonMode, @"marathon_object_id" },
            { RankingType.TokotonMode, @"tokoton_object_id" },
        };
    }
}
