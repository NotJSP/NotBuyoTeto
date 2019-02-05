using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkBuyoView : BuyoView {
        [SerializeField]
        private PhotonView photonView;

        public override void Glow() {
            photonView.RPC("GlowRPC", PhotonTargets.All);
        }

        public override void HideGlow() {
            photonView.RPC("HideGlowRPC", PhotonTargets.All);
        }

        public override void Destroy() {
            photonView.RPC("DestroyRPC", PhotonTargets.All);
        }

        [PunRPC]
        private void GlowRPC() {
            base.Glow();
        }

        [PunRPC]
        private void HideGlowRPC() {
            base.HideGlow();
        }

        [PunRPC]
        private void DestroyRPC() {
            base.Destroy();
        }
    }
}
