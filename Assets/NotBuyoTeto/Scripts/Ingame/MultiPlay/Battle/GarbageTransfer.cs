using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class GarbageTransfer : MonoBehaviour {
        private float elapsedTime;

        public void Initialize() {
            elapsedTime = 0.0f;
        }

        private void Update() {
            elapsedTime += Time.deltaTime;
        }

        public int GetGarbageCount(DeleteMinoInfo info, GameMode transferTo) {
            Debug.Log($"lines: {info.LineCount}, objects: {info.ObjectCount}");
            var lineAmount = (float)info.LineCount;
            var objectAmount = (float)info.ObjectCount / 7;
            var marginCf = 1.0f + 0.25f * (int)(elapsedTime / 60);
            var amount = (lineAmount + objectAmount) * marginCf;
            Debug.Log(amount);

            // TODO: モードに応じて量を変える

            return (int)amount;
        }
    }
}
