using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    public class RoomEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField]
        private RulePanel rulePanel;

        private string roomName;
        public string RoomName {
            get {
                return roomName;
            }
            set {
                this.roomName = value;
                updateView();
            }
        }
        public RoomSettings Settings;

        public void SetPanel(RulePanel panel) {
            this.rulePanel = panel;
        }

        public void OnPointerEnter(PointerEventData data) {
            rulePanel.Set(this);
        }

        public void OnPointerExit(PointerEventData data) {
        }

        private void updateView() {
            GetComponentInChildren<Text>().text = RoomName;
        }
    }
}
