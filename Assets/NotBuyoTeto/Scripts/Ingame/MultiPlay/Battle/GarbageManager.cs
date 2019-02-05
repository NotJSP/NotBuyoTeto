using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class GarbageManager : MonoBehaviour {
        [Header("References")]
        [SerializeField]
        private GarbageIndicator indicator;

        private int readyGarbageCount;
        public int ReadyGarbageCount {
            get {
                return readyGarbageCount;
            }
            protected set {
                readyGarbageCount = value;
                updateIndicator();
            }
        }

        public void Restart() {
            ReadyGarbageCount = 0;
        }

        public void Add(int amount) {
            ReadyGarbageCount += amount;
        }

        public int Subtract(int remain) {
            if (ReadyGarbageCount >= remain) {
                ReadyGarbageCount -= remain;
                remain = 0;
            } else {
                remain -= ReadyGarbageCount;
                ReadyGarbageCount = 0;
            }
            return remain;
        }

        private void updateIndicator() {
            indicator.Value = readyGarbageCount;
        }
    }
}
