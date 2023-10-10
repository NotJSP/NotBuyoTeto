using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class NetworkMinoFrame : MinoFrame {
        [SerializeField]
        private PhotonView photonView;

        public override void Set(MinoType type) {
            base.Set(type);
            photonView.RPC("Set", RpcTarget.OthersBuffered, type);
        }

        public override void Clear() {
            base.Clear();
            photonView.RPC("Clear", RpcTarget.OthersBuffered);
        }
    }
}
