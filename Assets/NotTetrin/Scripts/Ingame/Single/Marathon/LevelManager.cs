﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotTetrin.Ingame.Single.Marathon {
    public class LevelManager : MonoBehaviour {
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

        void Awake() {
            deletecount = 0;
            Value = 1;
            updateText();
        }

        public void DeleteCountUp() { 
            deletecount++;
            if(deletecount % 3 == 0) { //三列消すごとにレベルを1あげる。
                Value++;
                minoManager.fallSpeedUp();
                updateText();
            }
        }

        public void Reset() {
            deletecount = 0;
            level = 1;
            minoManager.defaultFallSpeed();
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