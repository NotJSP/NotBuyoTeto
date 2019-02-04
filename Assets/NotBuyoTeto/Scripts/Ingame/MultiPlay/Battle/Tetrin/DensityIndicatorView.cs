using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    [RequireComponent(typeof(SpriteRenderer))]
    public class DensityIndicatorView : MonoBehaviour {
        [SerializeField]
        private Vector3 offset;

        private new SpriteRenderer renderer;
        private Color color;

        private void Awake() {
            renderer = GetComponent<SpriteRenderer>();
            color = renderer.color;
        }

        private void Start() {
            transform.position += offset;
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.isWriting) {
                serialize(stream, info);
            } else {
                deserialize(stream, info);
            }
        }

        private void serialize(PhotonStream stream, PhotonMessageInfo info) {
            var alpha = renderer.color.a;
            stream.SendNext(alpha);
        }

        private void deserialize(PhotonStream stream, PhotonMessageInfo info) {
            var alpha = (float)stream.ReceiveNext();
            color.a = alpha;
            renderer.color = color;
        }
    }
}
