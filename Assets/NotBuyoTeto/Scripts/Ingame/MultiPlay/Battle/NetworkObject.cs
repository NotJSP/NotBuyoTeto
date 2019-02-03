using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkObject : MonoBehaviour {
        [SerializeField]
        private Vector3 offset;

        private PhotonView photonView;

        private void Awake() {
            this.photonView = GetComponent<PhotonView>();
            if (!photonView.isMine) {
                gameObject.tag = "NetworkObject";
                transform.position += offset;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.isWriting) {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            } else {
                transform.position = (Vector3)stream.ReceiveNext() + offset;
                transform.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
