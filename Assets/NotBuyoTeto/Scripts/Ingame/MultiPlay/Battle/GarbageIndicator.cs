using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class GarbageIndicator : MonoBehaviour {
        private Vector3 scale;

        private void Awake() {
            scale = transform.localScale;
        }

        public int Value {
            set {
                scale.y = 0.2f * value;
                transform.localScale = scale;
            }
        }
    }
}
