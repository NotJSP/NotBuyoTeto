using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkBuyo : Buyo {
        private PhotonView photonView;

        private void Awake() {
            photonView = GetComponent<PhotonView>();
        }

        protected override void Update() {
            if (!photonView.IsMine) { return; }
            base.Update();
        }

        protected override void OnCollisionEnter2D(Collision2D collision) {
            if (!photonView.IsMine) { return; }
            base.OnCollisionEnter2D(collision);
        }

        protected override void OnCollisionExit2D(Collision2D collision) {
            if (!photonView.IsMine) { return; }
            base.OnCollisionExit2D(collision);
        }
    }
}
