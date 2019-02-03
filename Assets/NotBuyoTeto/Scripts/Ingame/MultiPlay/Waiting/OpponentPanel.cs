using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class OpponentPanel : WaitingPanel {
        [Header("References")]
        [SerializeField]
        private Image tetrinImage;
        [SerializeField]
        private Image buyoImage;

        [Header("Appearance")]
        [SerializeField]
        private Color normalColor = Color.white;
        [SerializeField]
        private Color selectedColor = Color.white;
        [SerializeField]
        private Color decidedColor = Color.white;

        public event EventHandler<GameMode> OnDecideMode;

        private void OnEnable() {
            tetrinImage.color = normalColor;
            buyoImage.color = normalColor;
        }

        [PunRPC]
        public void SelectMode(GameMode mode) {
            changeColor(mode, selectedColor);
        }

        [PunRPC]
        public void DecideMode(GameMode mode) {
            changeColor(mode, decidedColor);
            OnDecideMode?.Invoke(this, mode);
        }

        private void changeColor(GameMode mode, Color color) {
            if (mode == GameMode.Tetrin) {
                tetrinImage.color = color;
                buyoImage.color = normalColor;
            }
            if (mode == GameMode.BuyoBuyo) {
                buyoImage.color = color;
                tetrinImage.color = normalColor;
            }
        }
    }
}