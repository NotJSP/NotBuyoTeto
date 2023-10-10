using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkBuyoView : BuyoView {
        [SerializeField]
        private PhotonView photonView;

        public override void Glow() {
            photonView.RPC("GlowRPC", RpcTarget.All);
        }

        public override void HideGlow() {
            photonView.RPC("HideGlowRPC", RpcTarget.All);
        }

        public override void Destroy() {
            photonView.RPC("DestroyRPC", RpcTarget.All);
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
