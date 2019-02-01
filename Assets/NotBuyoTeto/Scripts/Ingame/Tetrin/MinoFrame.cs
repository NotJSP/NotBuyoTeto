using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class MinoFrame : MonoBehaviour {
        [SerializeField]
        private MinoResolver resolver;
        [SerializeField]
        private SpriteRenderer container;

        private void Reset() {
            container = GetComponentInChildren<SpriteRenderer>();
        }

        public virtual void Clear() {
            container.sprite = null;
        }

        public virtual void Set(MinoType type) {
            Debug.Log(gameObject.path());
            Debug.Log($"MinoFrame::Set({type})");
            Debug.Log("container:" + container);
            Debug.Log("resolver: " + resolver);
            var mino = resolver.Get(type);
            var minoRenderer = mino.GetComponent<SpriteRenderer>();
            container.sprite = minoRenderer.sprite;
        }
    }
}
