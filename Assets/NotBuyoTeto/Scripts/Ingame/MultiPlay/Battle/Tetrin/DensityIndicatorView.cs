using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    [RequireComponent(typeof(SpriteRenderer))]
    public class DensityIndicatorView : MonoBehaviour, IPunObservable {
        [SerializeField]
        private Vector3 offset;

        private new SpriteRenderer renderer;
        private Color color;

        public void Awake() {
            renderer = GetComponent<SpriteRenderer>();
            color = renderer.color;
        }

        public void Start() {
            transform.position += offset;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                serialize(stream, info);
            } else {
                deserialize(stream, info);
            }
        }

        private void serialize(PhotonStream stream, PhotonMessageInfo info) {
            stream.SendNext(transform.localPosition + offset);
            stream.SendNext(transform.localScale);

            var alpha = renderer.color.a;
            stream.SendNext(alpha);
        }

        private void deserialize(PhotonStream stream, PhotonMessageInfo info) {
            transform.localPosition = (Vector3)stream.ReceiveNext();
            transform.localScale = (Vector3)stream.ReceiveNext();

            var alpha = (float)stream.ReceiveNext();
            color.a = alpha;
            renderer.color = color;
        }
    }
}