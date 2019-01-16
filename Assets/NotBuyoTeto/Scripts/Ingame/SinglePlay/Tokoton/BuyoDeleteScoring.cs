using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.SinglePlay.Tokoton {
    [RequireComponent(typeof(BuyoManager))]
    public class BuyoDeleteScoring : MonoBehaviour {
        [SerializeField]
        private Score score;
        
        public void buyoDeleteScoring(int combo) {
            var baseScore = 500.0f;
            var amount = baseScore * combo;
            score.Increase((int)amount);
        }
    }
}
