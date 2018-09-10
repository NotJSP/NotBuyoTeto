using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame {
    [DefaultExecutionOrder(10)]
    public class IncrementScore : MonoBehaviour {
        [SerializeField] private Text number;

        private bool elapsedFrame = true;
        private int amount;

        public void Add(int amount) {
            if (elapsedFrame) {
                this.amount = amount;
                elapsedFrame = false;
            } else {
                this.amount += amount;
            }
        }

        private void Update() {
            if (!elapsedFrame) {
                number.text = $"{amount}";
            }
            elapsedFrame = true;
        }
    }
}
