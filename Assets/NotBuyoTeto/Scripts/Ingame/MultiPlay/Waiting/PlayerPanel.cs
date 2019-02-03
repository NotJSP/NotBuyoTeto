using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class PlayerPanel : WaitingPanel {
        [SerializeField]
        private Button tetrinButton;
        [SerializeField]
        private Button buyoButton;
        [SerializeField]
        private GameObject modeContainer;
        [SerializeField]
        private PhotonView photonView;

        public event EventHandler<GameMode> OnDecideMode;

        public void SelectMode(GameMode mode) {
            photonView.RPC("SelectMode", PhotonTargets.OthersBuffered, mode);
            setProperties(mode);
        }

        public void DecideMode(GameMode mode) {
            photonView.RPC("DecideMode", PhotonTargets.OthersBuffered, mode);
            setProperties(mode);
            OnDecideMode?.Invoke(this, mode);
        }

        protected void setProperties(GameMode mode) {
            var properties = new Hashtable();
            properties["gamemode"] = mode;
            PhotonNetwork.player.SetCustomProperties(properties);
        }

        public void ModeContainerActivate(bool active) {
            modeContainer.SetActive(active);
        }
    }
}