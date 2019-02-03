using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkBuyoFrame : BuyoFrame {
        [SerializeField]
        private BuyoFrameView view;

        public override void Set(BuyoType[] types) {
            base.Set(types);
            view.photonView.RPC("Set", PhotonTargets.OthersBuffered, types);
        }
    }
}
