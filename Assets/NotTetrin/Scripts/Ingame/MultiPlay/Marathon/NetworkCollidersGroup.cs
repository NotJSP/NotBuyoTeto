using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkCollidersGroup : CollidersGroup {
        private PhotonView view;

        protected override void Awake() {
            view = GetComponent<PhotonView>();
            if (!view.IsMine) { return; }
            base.Awake();
        }

        protected override void Update() {
            if (!view.IsMine) { return; }
            base.Update();
        }

        public override void Initialize(Instantiator instantiator, GameObject wall) {
            if (!view.IsMine) { return; }
            base.Initialize(instantiator, wall);
        }

        public override void DeleteMino() {
            if (!view.IsMine) { return; }
            base.DeleteMino();
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
