using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    public class RulePanel : MonoBehaviour {
        [SerializeField]
        private Text opponentName;
        [SerializeField]
        private Text winsCount;
        [SerializeField]
        private Text fallSpeed;

        private void Awake() {
            Set(null);
        }

        public void Set(RoomEntry entry) {
            if (entry != null) {
                opponentName.text = entry.Name;
                winsCount.text = $"{entry.Settings.WinsCount}";
                fallSpeed.text = $"{entry.Settings.FallSpeed} m/s";
            } else {
                opponentName.text = "-";
                winsCount.text = "-";
                fallSpeed.text = "-";
            }
        }
    }
}
