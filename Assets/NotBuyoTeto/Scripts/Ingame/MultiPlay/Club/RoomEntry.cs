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

        public string Name;
        public RoomSettings Settings;

        private void OnValidate() {
            GetComponentInChildren<Text>().text = Name;
        }

        public void OnPointerEnter(PointerEventData data) {
            rulePanel.Set(this);
        }

        public void OnPointerExit(PointerEventData data) {
        }
    }
}
