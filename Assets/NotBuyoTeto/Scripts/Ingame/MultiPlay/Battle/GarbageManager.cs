using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public abstract class GarbageManager : MonoBehaviour {
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

        public bool IsFalling { get; protected set; }

        public virtual void Clear() {
            ReadyGarbageCount = 0;
        }

        public void Add(int amount) {
            ReadyGarbageCount += amount;
        }

        public abstract void Fall();

        private void updateIndicator() {
            indicator.Value = readyGarbageCount;
        }
    }
}
