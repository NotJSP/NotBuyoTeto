using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class WaitingPlayer {
        public readonly string Name;
        public readonly FightRecord FightRecord;
        public readonly int Rating;

        public WaitingPlayer(string name, FightRecord fightRecord, int rating) {
            this.Name = name;
            this.FightRecord = fightRecord;
            this.Rating = rating;
        }
    }
}
