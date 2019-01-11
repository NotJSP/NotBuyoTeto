using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class LevelManager : MonoBehaviour {
        [SerializeField]
        private Text label;

        public event EventHandler<int> ValueChanged;

        private static readonly int CountPerLevel = 4;

        private int deleteCount; //ぶよを消した回数
        private int level; //レベル
       
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

        public void DeleteCountUp() { 
            deleteCount += 1;

            var estimatedLevel = CalculateLevel(deleteCount);
            if (level != estimatedLevel) {
                Value = estimatedLevel;
            }
        }
        

        public void Initialize() {
            deleteCount = 0;
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