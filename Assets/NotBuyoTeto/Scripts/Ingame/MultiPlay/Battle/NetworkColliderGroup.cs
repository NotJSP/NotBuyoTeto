using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkColliderGroup : ColliderGroup {
        private PhotonView photonView;

        public override void Awake() {
            photonView = GetComponent<PhotonView>();
            if (!photonView.IsMine) { return; }
            base.Awake();
        }

        public override void Update() {
            if (!photonView.IsMine) { return; }
            base.Update();
        }

        public override void Initialize(Instantiator instantiator, GameObject wall) {
            if (!photonView.IsMine) { return; }
            base.Initialize(instantiator, wall);
        }

        public override void DeleteLine() {
            if (!photonView.IsMine) { return; }
            base.DeleteLine();
        }

        protected override void OnTriggerEnter2D(Collider2D collision) {
            if (!photonView.IsMine) { return; }
            base.OnTriggerEnter2D(collision);
        }

        protected override void OnTriggerExit2D(Collider2D collision) {
            if (!photonView.IsMine) { return; }
            base.OnTriggerExit2D(collision);
        }
    }
}
