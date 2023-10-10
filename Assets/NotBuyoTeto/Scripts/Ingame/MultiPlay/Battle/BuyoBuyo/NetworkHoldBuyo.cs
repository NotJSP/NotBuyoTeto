using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    public class NetworkHoldBuyo : HoldBuyo {
        [SerializeField]
        private PhotonView photonView;

        public override void Clear() {
            base.Clear();
            photonView.RPC("Clear", RpcTarget.Others);
        }

        public override void Set(Tuple<BuyoType, BuyoType> types) {
            base.Set(types);
            photonView.RPC("Set", RpcTarget.Others, types);
        }
    }
}
