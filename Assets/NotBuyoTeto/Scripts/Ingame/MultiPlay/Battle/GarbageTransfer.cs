using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;
using NotBuyoTeto.Ingame.Buyobuyo;
using NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin;
using NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    [RequireComponent(typeof(PhotonView))]
    public class GarbageTransfer : MonoBehaviour {
        [SerializeField]
        private TetoGarbageManager tetoGarbageManager;
        [SerializeField]
        private BuyoGarbageManager buyoGarbageManager;

        private GameMode mode;
        private float elapsedTime;

        public void Initialize(GameMode mode) {
            this.mode = mode;
        }

        public void GameStart() {
            this.elapsedTime = 0.0f;
        }

        private void Update() {
            elapsedTime += Time.deltaTime;
        }

        public void Transfer(GameMode to, int count) {
            if (to == GameMode.Tetrin) { tetoGarbageManager.Add(count); }
            if (to == GameMode.BuyoBuyo) { buyoGarbageManager.Add(count); }
        }

        private float marginCoefficient => 1.0f + 0.25f * (int)(elapsedTime / 60);

        public int GetGarbageCount(DeleteMinoInfo info, GameMode transferTo) {
            // 1.25 * LineCount
            var lineAmount = 1.25f * info.LineCount;
            // 0.167 * ObjectCount
            var objectAmount = 0.167f * info.ObjectCount;
            var amount = (lineAmount + objectAmount) * marginCoefficient;

            // 対Buyoでは1.5倍
            if (transferTo == GameMode.BuyoBuyo) { amount *= 1.5f; }
            Debug.Log("Total Amount (Garbages): " + amount);

            return (int)amount;
        }

        public int GetGarbageCount(DeleteBuyoInfo info, GameMode transferTo) {
            // 0.1 * ( ObjectCount ^ 2 - 10 )
            var objectAmount = 0.1f * (Mathf.Pow(info.ObjectCount, 2) - 10);
            // 0.38 * ComboCount ^ 1.25
            var comboAmount = 0.38f * Mathf.Pow(info.ComboCount, 1.25f);
            var amount = (objectAmount + comboAmount) * marginCoefficient;

            // 対Buyoでは1.25倍
            if (transferTo == GameMode.BuyoBuyo) { amount *= 1.25f; }
            Debug.Log("Total Amount (Garbages): " + amount);

            return (int)amount;
        }

        [PunRPC]
        private void OnDeleteLineOpponent(int lineCount, int objectCount) {
            var info = new DeleteMinoInfo(lineCount, objectCount);
            var count = GetGarbageCount(info, mode);
            Transfer(mode, count);
        }

        [PunRPC]
        private void OnDeleteBuyoOpponent(int objectCount, int comboCount) {
            var info = new DeleteBuyoInfo(objectCount, comboCount);
            var count = GetGarbageCount(info, mode);
            Transfer(mode, count);
        }
    }
}
