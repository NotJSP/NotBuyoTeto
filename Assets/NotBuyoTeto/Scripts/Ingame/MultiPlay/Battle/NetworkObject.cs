using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    [RequireComponent(typeof(PhotonView))]
    public class NetworkObject : MonoBehaviour, IPunObservable {
        [SerializeField]
        private Vector3 offset;

        private PhotonView photonView;

        private Vector3 targetPosition;
        private Quaternion targetRotation;

        public void Awake() {
            this.photonView = GetComponent<PhotonView>();
            if (!photonView.IsMine) {
                gameObject.tag = "NetworkObject";
                transform.position += offset;
            }
        }

        public void Update() {
            if (photonView.IsMine) { return; }
            var distance = Vector3.Distance(transform.position, targetPosition);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, distance * Time.deltaTime * PhotonNetwork.SerializationRate);
            var angle = Quaternion.Angle(transform.rotation, targetRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angle * Time.deltaTime * PhotonNetwork.SerializationRate);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                stream.SendNext(transform.position + offset);
                stream.SendNext(transform.rotation);
            } else {
                var firstRecieve = false;
                if (targetPosition == null || targetRotation == null) {
                    firstRecieve = true;
                }
                targetPosition = (Vector3)stream.ReceiveNext();
                targetRotation = (Quaternion)stream.ReceiveNext();
                if (firstRecieve) {
                    transform.position = targetPosition;
                    transform.rotation = targetRotation;
                }
            }
        }
    }
}
