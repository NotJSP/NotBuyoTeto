using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    [RequireComponent(typeof(PhotonView), typeof(HoldMinoView))]
    public class NetworkHoldMinoView : MonoBehaviour {
        [SerializeField]
        private HoldMinoView view;

        private void Reset() {
            view = GetComponent<HoldMinoView>();
        }

        [PunRPC]
        public void Clear() {
            view.Clear();
        }

        [PunRPC]
        public void Set(MinoType type) {
            view.Set(type);
        }
    }
}
