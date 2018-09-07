using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkCollidersGroup : CollidersGroup {
        private PhotonView view;

        protected override void Awake() {
            view = GetComponent<PhotonView>();
            if (!view.isMine) { return; }
            base.Awake();
        }

        protected override void Update() {
            if (!view.isMine) { return; }
            base.Update();
        }

        public override void Initialize(Instantiator instantiator, GameObject wall) {
            if (!view.isMine) { return; }
            base.Initialize(instantiator, wall);
        }

        public override void DeleteMino() {
            if (!view.isMine) { return; }
            base.DeleteMino();
        }

        protected override void OnTriggerEnter2D(Collider2D collision) {
            if (!view.isMine) { return; }
            base.OnTriggerEnter2D(collision);
        }

        protected override void OnTriggerExit2D(Collider2D collision) {
            if (!view.isMine) { return; }
            base.OnTriggerExit2D(collision);
        }
    }
}
