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

        public void Transfer(GameMode to, int count) {
            Debug.Log($"Transfer: (to: {to}, count: {count})");
            if (to == GameMode.Tetrin) {
                tetoGarbageManager.Add(count);
            }
            if (to == GameMode.BuyoBuyo) {
                buyoGarbageManager.Add(count);
            }
        }

        public int GetGarbageCount(DeleteMinoInfo info, GameMode transferTo) {
            var lineAmount = 1.25f * info.LineCount;
            var objectAmount = 0.167f * info.ObjectCount;
            var marginCf = 1.0f + 0.25f * (int)(elapsedTime / 60);
            var amount = (lineAmount + objectAmount) * marginCf;
            Debug.Log("GarbageCount: " + amount);

            // TODO: モードに応じて量を変える

            return (int)amount;
        }

        public int GetGarbageCount(DeleteBuyoInfo info, GameMode transferTo) {
            var objectAmount = 0.25f * info.ObjectCount;
            var comboAmount = 0.67f * info.ComboCount;
            var marginCf = 1.0f + 0.25f * (int)(elapsedTime / 60);
            var amount = (objectAmount + objectAmount) * marginCf;
            Debug.Log("GarbageCount: " + amount);

            // TODO: モードに応じて量を変える

            return (int)amount;
        }
    }
}
