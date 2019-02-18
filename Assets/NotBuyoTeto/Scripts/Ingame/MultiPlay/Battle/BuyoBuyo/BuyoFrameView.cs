using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    [RequireComponent(typeof(PhotonView), typeof(BuyoFrame))]
    public class BuyoFrameView : MonoBehaviour {
        private BuyoFrame frame;

        private void Awake() {
            this.frame = GetComponent<BuyoFrame>();
        }

        [PunRPC]
        public virtual void Set(BuyoType type1, BuyoType type2) {
            var types = new Tuple<BuyoType, BuyoType>(type1, type2);
            frame.Set(types);
        }
    }
}
