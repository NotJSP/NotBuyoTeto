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
            Clear();
        }

        public void Clear() {
            opponentName.text = "-";
            winsCount.text = "-";
            fallSpeed.text = "-";
        }

        public void Set(RoomEntry entry) {
            if (entry == null) {
                Clear();
                return;
            }

            opponentName.text = IdentificationNameUtility.ParseName(entry.RoomName);
            winsCount.text = $"{entry.Settings.WinsCount}";
            fallSpeed.text = $"{entry.Settings.FallSpeed} m/s";
        }
    }
}
