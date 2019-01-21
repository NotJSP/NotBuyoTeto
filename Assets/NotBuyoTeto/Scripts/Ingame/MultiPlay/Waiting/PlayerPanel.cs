using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class PlayerPanel : WaitingPanel {
        [SerializeField]
        private Button tetrinButton;
        [SerializeField]
        private Button buyoButton;
        [SerializeField]
        private PhotonView photonView;

        public void SelectMode(GameMode mode) {
            photonView.RPC("SelectMode", PhotonTargets.OthersBuffered, mode);
        }

        public void DecideMode(GameMode mode) {
            photonView.RPC("DecideMode", PhotonTargets.OthersBuffered, mode);
        }
    }
}