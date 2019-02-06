using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class NetworkMinoFrame : MinoFrame {
        [SerializeField]
        private PhotonView photonView;

        public override void Set(MinoType type) {
            base.Set(type);
            photonView.RPC("Set", PhotonTargets.OthersBuffered, type);
        }

        public override void Clear() {
            base.Clear();
            photonView.RPC("Clear", PhotonTargets.OthersBuffered);
        }
    }
}
