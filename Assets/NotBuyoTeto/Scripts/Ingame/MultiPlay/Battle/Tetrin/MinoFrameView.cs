using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class MinoFrameView : PunBehaviour {
        [SerializeField]
        private MinoResolver resolver;
        [SerializeField]
        private SpriteRenderer container;

        private void Reset() {
            container = GetComponentInChildren<SpriteRenderer>();
        }

        [PunRPC]
        public void Clear() {
            container.sprite = null;
        }

        [PunRPC]
        public void Set(MinoType type) {
            var mino = resolver.Get(type);
            var minoRenderer = mino.GetComponent<SpriteRenderer>();
            container.sprite = minoRenderer.sprite;
        }
    }
}
