using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class FightRecord {
        public int FightCount { get; private set; }
        public int WinCount { get; private set; }
        public int LoseCount => FightCount - WinCount;

        public float WinRate {
            get {
                if (WinCount == 0) { return 0f; }
                return (float)WinCount / FightCount;
            }
        }

        public FightRecord(int fightCount, int winCount) {
            this.FightCount = fightCount;
            this.WinCount = winCount;
        }

        public FightRecord Win() {
            this.FightCount++;
            this.WinCount++;
            return this;
        }

        public FightRecord Lose() {
            this.FightCount++;
            return this;
        }
    }
}
