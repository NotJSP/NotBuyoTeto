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
            // LineCount ^ 1.33
            var lineAmount = Mathf.Pow(info.LineCount, 1.33f);
            // 0.12 * ObjectCount
            var objectAmount = 0.12f * info.ObjectCount;
            var amount = (lineAmount + objectAmount) * marginCoefficient;

            // 対Buyoでは1.67倍
            if (transferTo == GameMode.BuyoBuyo) { amount *= 1.67f; }
            Debug.Log("Total Amount (Garbages): " + amount);

            return (int)amount;
        }

        public int GetGarbageCount(DeleteBuyoInfo info, GameMode transferTo) {
            // 0.2 * ( ObjectCount ^ 1.5 - 6 )
            var objectAmount = 0.2f * (Mathf.Pow(info.ObjectCount, 1.5f) - 6);
            // 0.5 * ComboCount ^ 1.2
            var comboAmount = 0.5f * Mathf.Pow(info.ComboCount, 1.2f);
            var amount = (objectAmount + comboAmount) * marginCoefficient;

            // 対Tetoでは0.9倍
            if (transferTo == GameMode.Tetrin) { amount *= 0.9f; }
            // 対Buyoでは1.5倍
            if (transferTo == GameMode.BuyoBuyo) { amount *= 1.5f; }
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
