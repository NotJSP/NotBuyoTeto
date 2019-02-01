using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class NetworkMinoFrame : MinoFrame {
        [SerializeField]
        private MinoFrameView view;

        public override void Set(MinoType type) {
            base.Set(type);
            view.photonView.RPC("Set", PhotonTargets.OthersBuffered, type);
        }

        public override void Clear() {
            base.Clear();
            view.photonView.RPC("Clear", PhotonTargets.OthersBuffered);
        }
    }
}
