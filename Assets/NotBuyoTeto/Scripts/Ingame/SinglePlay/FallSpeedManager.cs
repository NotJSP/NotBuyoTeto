using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.SinglePlay {
    public class FallSpeedManager : MonoBehaviour {
        [SerializeField]
        private float defaultSpeed;
        public float DefaultSpeed => defaultSpeed;

        private float increaseSpeed(int level) {
            return level * (level - 1) * 0.0075f;
        }

        public float GetSpeed(int level) {
            return defaultSpeed + increaseSpeed(level);
        }
    }
}
