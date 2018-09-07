using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.Ingame.SinglePlay {
    public class Score : MonoBehaviour {
        [SerializeField] private Text text;
        [SerializeField] private IncrementScore incrementScore;

        private Animator animator;
        private int value;

        public event EventHandler ValueChanged;
        public int Value {
            get {
                return value;
            }
            private set {
                this.value = value;
                updateText();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Awake() {
            animator = GetComponent<Animator>();
            updateText();
        }

        public void Reset() {
            Value = 0;
        }

        public void Increase(int amount) {
            Value += amount;

            if (amount >= 75) {
                incrementScore.Add(amount);
                animator.Play(@"ScoreIncrement", 0, 0.0f);
            }
        }

        private void updateText() {
            text.text = string.Format("{0:0000000}", value);
        }
    }
}