using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotTetrin.Ingame.SinglePlay {
    public class Ranker {
        public string Name { get; }
        public int Score { get; }

        public Ranker(string name, int score) {
            this.Name = name;
            this.Score = score;
        }

        public override string ToString() {
            return $"{Name}: {Score}";
        }
    }
}
