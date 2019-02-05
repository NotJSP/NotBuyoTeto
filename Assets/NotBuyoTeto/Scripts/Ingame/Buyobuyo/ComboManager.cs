using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [RequireComponent(typeof(ComboView))]
    public class ComboManager : MonoBehaviour {
        [SerializeField]
        private ComboView view;

        public float resetTime;
        private float elapsedTime;

        public int Value { get; private set; }

        private void Reset() {
            view = GetComponent<ComboView>();
        }

        private void Update() {
            if (Value > 0) {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > resetTime) { ResetValue(); }
            }
        }

        private void ResetValue() {
            elapsedTime = 0.0f;
            Value = 0;
        }

        public void CountUp() {
            elapsedTime = 0.0f;
            Value++;
            Debug.Log(Value + "Combo");
        }

        public void Show(Vector2 position) {
            view.Show(position, Value);
        }
    }  
}