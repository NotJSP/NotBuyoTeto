using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class GarbageIndicator : MonoBehaviour {
        [SerializeField]
        private PhotonView view;

        private Vector3 scale;

        private void Awake() {
            scale = transform.localScale;
        }

        public int Value {
            set {
                SetValue(value);
                view?.RPC("SetValue", RpcTarget.OthersBuffered, value);
            }
        }

        [PunRPC]
        public void SetValue(int value) {
            scale.y = 0.2f * value;
            transform.localScale = scale;
        }
    }
}
