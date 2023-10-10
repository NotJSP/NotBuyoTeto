using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkColliderGroup : ColliderGroup {
        private PhotonView view;

        public override void Awake() {
            view = GetComponent<PhotonView>();
            if (!view.IsMine) { return; }
            base.Awake();
        }

        public override void Update() {
            if (!view.IsMine) { return; }
            base.Update();
        }

        public override void Initialize(Instantiator instantiator, GameObject wall) {
            if (!view.IsMine) { return; }
            base.Initialize(instantiator, wall);
        }

        public override void DeleteLine() {
            if (!view.IsMine) { return; }
            base.DeleteLine();
        }

        protected override void OnTriggerEnter2D(Collider2D collision) {
            if (!view.IsMine) { return; }
            base.OnTriggerEnter2D(collision);
        }

        protected override void OnTriggerExit2D(Collider2D collision) {
            if (!view.IsMine) { return; }
            base.OnTriggerExit2D(collision);
        }
    }
}
