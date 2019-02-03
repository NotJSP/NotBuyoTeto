using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class NetworkHoldMino : HoldMino {
        [SerializeField]
        private PhotonView photonView;

        public override void Clear() {
            base.Clear();
            photonView.RPC("Clear", PhotonTargets.Others);
        }

        public override void Set(MinoType type) {
            base.Set(type);
            photonView.RPC("Set", PhotonTargets.Others, type);
        }
    }
}
