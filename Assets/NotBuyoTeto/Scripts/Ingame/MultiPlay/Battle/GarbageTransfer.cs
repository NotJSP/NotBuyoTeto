using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    [RequireComponent(typeof(PhotonView))]
    public class GarbageTransfer : MonoBehaviour {
        [SerializeField]
        private GarbageManager garbageManager;

        private PhotonView photonView;

        private void Awake() {
            photonView = GetComponent<PhotonView>();
        }

        public void Send(int count) {
            photonView.RPC("recieveGarbage", RpcTarget.Others, count);
        }

        [PunRPC]
        private void recieveGarbage(int count) {
            garbageManager.Add(count);
        }
    }
}
