using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class PlayerStats {
        public int Rating { get; private set; }
        public int BattleCount { get; private set; }
        public int WinCount { get; private set; }
        public int LoseCount => BattleCount - WinCount;

        public float WinRate {
            get {
                if (BattleCount == 0) { return 0f; }
                return (float)WinCount / BattleCount;
            }
        }

        public PlayerStats(int rating, int battleCount, int winCount) {
            this.Rating = rating;
            this.BattleCount = battleCount;
            this.WinCount = winCount;
        }

        public PlayerStats Win() {
            this.BattleCount++;
            this.WinCount++;
            return this;
        }

        public PlayerStats Lose() {
            this.BattleCount++;
            return this;
        }
    }
}
