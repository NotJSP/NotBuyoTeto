using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkHoldMino : HoldMino {
        private PhotonView photonView;

        protected override void Awake() {
            base.Awake();
            photonView = GetComponent<PhotonView>();
        }

        public override void Push(MinoType type) {
            base.Push(type);
            photonView.RPC("OnPush", PhotonTargets.OthersBuffered, type);
        }
    }
}
