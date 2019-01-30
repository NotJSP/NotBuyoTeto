using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.SinglePlay.Tokoton {
    [RequireComponent(typeof(BuyoManager))]
    public class BuyoDeleteScoring : MonoBehaviour {
        [SerializeField]
        private ScoreManager score;
        
        public void buyoDeleteScoring(int combo, int level) {
            var baseScore = 200.0f;
            var amount = baseScore + (combo * level * 50);
            score.Increase((int)amount);
        }
    }
}
