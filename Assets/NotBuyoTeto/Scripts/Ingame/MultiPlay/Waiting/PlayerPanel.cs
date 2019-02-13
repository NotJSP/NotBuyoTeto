using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class PlayerPanel : WaitingPanel {
        [SerializeField]
        private GameObject modeContainer;
        [SerializeField]
        private GameModeButton tetrinButton;
        [SerializeField]
        private GameModeButton buyoButton;
        [SerializeField]
        private PhotonView photonView;

        private Dictionary<GameMode, GameModeButton> buttons = new Dictionary<GameMode, GameModeButton>();

        public GameMode? SelectedMode { get; private set; }

        private void Awake() {
            buttons[GameMode.Tetrin] = tetrinButton;
            buttons[GameMode.BuyoBuyo] = buyoButton;
        }

        public void DecideRandomMode() {
            var mode = SelectedMode.HasValue ? SelectedMode.Value : (GameMode)Random.Range(0, 2);
            SelectMode(mode);
            DecideMode();
        }

        public void SelectMode(GameMode mode) {
            buttons[mode].Select();
            SelectedMode = mode;
            photonView.RPC("SelectMode", PhotonTargets.OthersBuffered, mode);
        }

        public void DecideMode() {
            var mode = SelectedMode.Value;
            buttons[mode].Decide();
            setProperties(mode);
            photonView.RPC("DecideMode", PhotonTargets.OthersBuffered, mode);
        }

        protected void setProperties(GameMode mode) {
            var properties = new Hashtable();
            properties["mode"] = mode;
            PhotonNetwork.player.SetCustomProperties(properties);
        }

        public void ModeContainerActivate(bool active) {
            modeContainer.SetActive(active);
        }
    }
}