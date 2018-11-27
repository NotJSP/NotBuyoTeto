using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class LevelManager : MonoBehaviour {
        [SerializeField]
        private Text levelText;
        [SerializeField]
        private BuyoManager buyoManager;

        private int deletecount; //ミノを消した列数。
        private int level; //レベル
       
        public int Value {
            get {
                return level;
            }
            private set {
                level = value;
                updateText();
            }
        }

        private void Awake() {
            deletecount = 0;
            Value = 1;
            updateText();
        }

        public void DeleteCountUp() { 
            deletecount++;
            if(deletecount % 3 == 0) { //三列消すごとにレベルを1あげる。
                Value++;
                buyoManager.fallSpeedUp(level);
                updateText();
            }
        }

        public void Reset() {
            deletecount = 0;
            level = 1;
            updateText();
        }

        public int getLevel() {
            return level;
        }
        
        private void updateText() {
            levelText.text = string.Format("{0:000}", level);
        }
    }
}