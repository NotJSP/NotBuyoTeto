using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkColliderGroup : ColliderGroup {
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

        public override void DeleteLine() {
            if (!view.isMine) { return; }
            base.DeleteLine();
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
