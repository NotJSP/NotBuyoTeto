﻿using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class GarbageCalculator : MonoBehaviour {
        [SerializeField]
        private GameTimer timer;

        // 経過時間による補正
        private float marginCoefficient => 1.0f + 0.25f * (int)(this.timer.ElapsedTime / 60);

        public GarbageCalculator(GameTimer timer) {
            this.timer = timer;
        }

        public int Calculate(DeleteMinoInfo info, GameMode transferTo) {
            // LineCount ^ 1.33
            var lineAmount = Mathf.Pow(info.LineCount, 1.33f);
            // 0.12 * ObjectCount
            var objectAmount = 0.12f * info.ObjectCount;
            var amount = (lineAmount + objectAmount) * marginCoefficient;

            // 対Buyoでは1.5倍
            if (transferTo == GameMode.BuyoBuyo) { amount *= 1.5f; }

            Debug.Log($"Lines: {info.LineCount}, Objects: {info.ObjectCount} ===> Amount: {amount}");
            return (int)amount;
        }

        public int Calculate(DeleteBuyoInfo info, GameMode transferTo) {
            // 0.2 * ( ObjectCount ^ 1.5 - 6 )
            var objectAmount = 0.2f * (Mathf.Pow(info.ObjectCount, 1.5f) - 6);
            // 0.6 * ComboCount ^ 1.35
            var comboAmount = 0.6f * Mathf.Pow(info.ComboCount, 1.35f);
            var amount = (objectAmount + comboAmount) * marginCoefficient;

            // 対Buyoでは1.35倍
            if (transferTo == GameMode.BuyoBuyo) { amount *= 1.35f; }

            Debug.Log($"Objects: {info.ObjectCount}, Combos: {info.ComboCount} ===> Amount: {amount}");
            return (int)amount;
        }
    }
}
