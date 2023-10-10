using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class NetworkHoldMino : HoldMino {
        [SerializeField]
        private PhotonView photonView;

        public override void Clear() {
            base.Clear();
            photonView.RPC("Clear", RpcTarget.Others);
        }

        public override void Set(MinoType type) {
            base.Set(type);
            photonView.RPC("Set", RpcTarget.Others, type);
        }
    }
}
