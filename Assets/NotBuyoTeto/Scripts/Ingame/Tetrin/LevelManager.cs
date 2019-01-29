using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class LevelManager : MonoBehaviour {
        [SerializeField]
        private Text label;

        public event EventHandler<int> ValueChanged;

        private static readonly int CountPerLevel = 4;

        private int increaseLevel;
        private int deleteLines;

        private int level;  // レベル
        public int Value {
            get {
                return level;
            }
            private set {
                level = value;
                ValueChanged?.Invoke(this, value);
                updateText();
            }
        }

        public void CountUp(int lines) { 
            deleteLines += lines;

            var estimatedLevel = CalculateLevel(deleteLines) + increaseLevel;
            if (level != estimatedLevel) {
                Value = estimatedLevel;
            }
        }

        public void Increase(int amount) {
            increaseLevel += amount;
            Value += amount;
        }

        public void Initialize() {
            deleteLines = 0;
            increaseLevel = 0;
            Value = 1;
        }
        
        private void updateText() {
            label.text = string.Format("{0:000}", level);
        }

        public int CalculateLevel(int deleteCount) {
            return 1 + deleteCount / CountPerLevel;
        }
    }
}