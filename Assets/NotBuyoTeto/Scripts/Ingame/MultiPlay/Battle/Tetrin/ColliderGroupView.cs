using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    [RequireComponent(typeof(Renderer))]
    public class ColliderGroupView : MonoBehaviour {
        private new Renderer renderer;
        private Renderer Renderer {
            get {
                if (renderer == null) {
                    renderer = GetComponent<Renderer>();
                }
                return renderer;
            }
        }

        private bool firstRead = true;
        private bool firstWrite = true;

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.isWriting) {
                serialize(stream, info);
            } else {
                deserialize(stream, info);
            }
        }

        private void serialize(PhotonStream stream, PhotonMessageInfo info) {
            if (firstWrite) {
                serializeFirst(stream, info);
            }
            var enabled = Renderer.enabled;
            stream.SendNext(enabled);
        }

        private void serializeFirst(PhotonStream stream, PhotonMessageInfo info) {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localScale);
            firstWrite = false;
        }

        private void deserialize(PhotonStream stream, PhotonMessageInfo info) {
            if (firstRead) {
                deserializeFirst(stream, info);
            }
            var enabled = (bool)stream.ReceiveNext();
            Renderer.enabled = enabled;
        }

        private void deserializeFirst(PhotonStream stream, PhotonMessageInfo info) {
            var parent = GameObject.Find(@"Perspectives/Opponent Side/Teto Perspective/Collider Field").transform;
            transform.SetParent(parent);

            transform.localPosition = (Vector3)stream.ReceiveNext();
            transform.localScale = (Vector3)stream.ReceiveNext();
            firstRead = false;
        }
    }
}