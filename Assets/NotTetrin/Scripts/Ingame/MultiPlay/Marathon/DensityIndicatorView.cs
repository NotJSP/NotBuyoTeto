using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame.MultiPlay.Marathon {
    [RequireComponent(typeof(SpriteRenderer))]
    public class DensityIndicatorView : MonoBehaviour {
        private new SpriteRenderer renderer;
        private Color color;

        private void Awake() {
            renderer = GetComponent<SpriteRenderer>();
            color = renderer.color;
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
