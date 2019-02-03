using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    [RequireComponent(typeof(Button))]
    public class RoomEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField]
        private RulePanel rulePanel;

        private ClubManager clubManager;
        private Button button;

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

        public event EventHandler PressedButton;

        private void Awake() {
            button = GetComponentInChildren<Button>();
            button.onClick.AddListener(OnPressedButton);
        }

        public void SetPanel(ClubManager clubManager, RulePanel panel) {
            this.clubManager = clubManager;
            this.rulePanel = panel;
        }

        public void OnPressedButton() {
            clubManager.StartByGuest(RoomName);
            PressedButton?.Invoke(this, EventArgs.Empty);
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
