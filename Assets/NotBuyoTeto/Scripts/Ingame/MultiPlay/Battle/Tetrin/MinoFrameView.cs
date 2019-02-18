using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    [RequireComponent(typeof(PhotonView), typeof(MinoFrame))]
    public class MinoFrameView : MonoBehaviour {
        [SerializeField]
        private MinoFrame frame;

        private void Awake() {
            frame = GetComponent<MinoFrame>();
        }

        [PunRPC]
        public void Clear() {
            frame.Clear();
        }

        [PunRPC]
        public void Set(MinoType type) {
            frame.Set(type);
        }
    }
}
