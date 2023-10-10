using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkBuyoFrame : BuyoFrame {
        [SerializeField]
        private PhotonView photonView;

        public override void Set(Tuple<BuyoType, BuyoType> types) {
            base.Set(types);
            photonView.RPC("Set", RpcTarget.OthersBuffered, types.Item1, types.Item2);
        }
    }
}
