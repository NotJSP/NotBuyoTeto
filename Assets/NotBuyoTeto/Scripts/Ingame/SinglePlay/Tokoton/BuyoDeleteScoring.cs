using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.SinglePlay.Tokoton {
    public class BuyoDeleteScoring : MonoBehaviour {
        [SerializeField]
        private ScoreManager score;
        
        public void buyoDeleteScoring(int level, DeleteBuyoInfo info) {
            var baseScore = 200.0f;
            var levelsAmount = 1.0f + 0.15f * Mathf.Pow(level, 1.05F);
            var combosAmount = 1.5 * Mathf.Pow(info.ComboCount, 2f);
            var objectsAmount = Mathf.Pow((info.ObjectCount - 3), 0.25f);
            var amount = baseScore * levelsAmount * combosAmount * objectsAmount;
            Debug.Log("Score: " + amount);
            score.Increase((int)amount);
        }
    }
}
