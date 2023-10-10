using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class WaitingPlayer {
        public readonly string Name;
        public readonly PlayerStats Stats;

        public WaitingPlayer(string name, PlayerStats stats) {
            this.Name = name;
            this.Stats = stats;
        }
    }
}
