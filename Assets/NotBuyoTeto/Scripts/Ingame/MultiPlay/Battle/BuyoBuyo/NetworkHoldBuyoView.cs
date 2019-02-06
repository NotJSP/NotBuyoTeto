using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    [RequireComponent(typeof(PhotonView), typeof(HoldBuyoView))]
    public class NetworkHoldBuyoView : MonoBehaviour {
        [SerializeField]
        private HoldBuyoView view;

        private void Reset() {
            view = GetComponent<HoldBuyoView>();
        }

        [PunRPC]
        public void Clear() {
            view.Clear();
        }

        [PunRPC]
        public void Set(Tuple<BuyoType, BuyoType> types) {
            view.Set(types);
        }
    }
}
