using System;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame {
    public class Ceiling : MonoBehaviour {
        public bool IsHit { get; private set; } = false;
        private int hitCount = 0;

        public void OnTriggerEnter2D(Collider2D collision) {
            if (hitCount == 0) {
                IsHit = true;
            }
            hitCount++;
        }

        public void OnTriggerExit2D(Collider2D collision) {
            hitCount--;
            if (hitCount == 0) {
                IsHit = false;
            }
        }
    }
}
