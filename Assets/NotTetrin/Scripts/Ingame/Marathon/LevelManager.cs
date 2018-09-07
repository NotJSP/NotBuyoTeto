using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.Ingame.Marathon {
    public class LevelManager : MonoBehaviour {
        [SerializeField] private GroupManager groupManager;
        [SerializeField] private Text levelText;
        [SerializeField] private MinoManager minoManager;

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
            groupManager.MinoDeleted += onMinoDeleted;
            deletecount = 0;
            Value = 1;
            updateText();
        }

        private void onMinoDeleted(object sender, DeleteMinoInfo info) {
            for (int i = 0; i < info.LineCount; i++) {
                DeleteCountUp();
            }
        }

        public void DeleteCountUp() { 
            deletecount++;
            if(deletecount % 3 == 0) { //三列消すごとにレベルを1あげる。
                Value++;
                minoManager.fallSpeedUp(level);
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