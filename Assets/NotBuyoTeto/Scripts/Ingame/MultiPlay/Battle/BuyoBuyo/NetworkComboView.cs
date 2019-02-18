using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    [RequireComponent(typeof(ComboView), typeof(PhotonView))]
    public class NetworkComboView : ComboView {
        [SerializeField]
        private Vector2 offset;

        private PhotonView photonView;

        private void Awake() {
            photonView = GetComponent<PhotonView>();
        }

        public override void Show(Vector2 position, int value) {
            base.Show(position, value);
            photonView.RPC("ShowRPC", PhotonTargets.Others, position, value);
        }

        [PunRPC]
        private void ShowRPC(Vector2 position, int value) {
            position += offset;
            base.Show(position, value);
        }
    }
}