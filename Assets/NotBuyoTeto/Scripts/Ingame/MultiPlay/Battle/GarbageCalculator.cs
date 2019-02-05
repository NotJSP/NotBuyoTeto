using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class GarbageCalculator : MonoBehaviour {
        private float elapsedTime;

        private float marginCoefficient => 1.0f + 0.25f * (int)(elapsedTime / 60);

        public void Restart() {
            elapsedTime = 0.0f;
        }

        private void Update() {
            elapsedTime += Time.deltaTime;
        }

        public int Calculate(DeleteMinoInfo info, GameMode transferTo) {
            // LineCount ^ 1.33
            var lineAmount = Mathf.Pow(info.LineCount, 1.33f);
            // 0.12 * ObjectCount
            var objectAmount = 0.12f * info.ObjectCount;
            var amount = (lineAmount + objectAmount) * marginCoefficient;

            // 対Buyoでは1.8倍
            if (transferTo == GameMode.BuyoBuyo) { amount *= 1.8f; }

            return (int)amount;
        }

        public int Calculate(DeleteBuyoInfo info, GameMode transferTo) {
            // 0.2 * ( ObjectCount ^ 1.5 - 6 )
            var objectAmount = 0.2f * (Mathf.Pow(info.ObjectCount, 1.5f) - 6);
            // 0.45 * ComboCount ^ 1.2
            var comboAmount = 0.45f * Mathf.Pow(info.ComboCount, 1.2f);
            var amount = (objectAmount + comboAmount) * marginCoefficient;

            // 対Tetoでは0.89倍
            if (transferTo == GameMode.Tetrin) { amount *= 0.89f; }
            // 対Buyoでは1.5倍
            if (transferTo == GameMode.BuyoBuyo) { amount *= 1.5f; }

            return (int)amount;
        }
    }
}
