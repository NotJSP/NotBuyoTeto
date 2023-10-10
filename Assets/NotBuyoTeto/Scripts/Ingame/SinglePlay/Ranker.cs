using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotBuyoTeto.Ingame.SinglePlay {
    public class Ranker {
        public string UserId { get; }
        public int Score { get; }

        public Ranker(string userId, int score) {
            this.UserId = userId;
            this.Score = score;
        }

        public override string ToString() {
            return $"{UserId}: {Score}";
        }
    }
}
