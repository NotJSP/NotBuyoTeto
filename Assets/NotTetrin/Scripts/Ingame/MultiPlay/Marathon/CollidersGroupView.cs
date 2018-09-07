using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotTetrin.Utility;
using Photon.Pun;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    [RequireComponent(typeof(Renderer))]
    public class CollidersGroupView : MonoBehaviour {
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
            if (stream.IsWriting) {
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
            var parent_path = transform.parent.path();
            stream.SendNext(parent_path);
            var position = transform.position;
            stream.SendNext(position);
            var scale = transform.localScale;
            stream.SendNext(scale);

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
            var parent_path = (string)stream.ReceiveNext();
            transform.parent = GameObject.Find(parent_path).transform;
            var position = (Vector3)stream.ReceiveNext();
            transform.position = position;
            var scale = (Vector3)stream.ReceiveNext();
            transform.localScale = scale;

            firstRead = false;
        }
    }
}