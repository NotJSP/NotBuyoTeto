using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class MinoFrame : MonoBehaviour {
        [SerializeField]
        private MinoResolver resolver;
        [SerializeField]
        private SpriteRenderer container;

        private void Reset() {
            container = GetComponentInChildren<SpriteRenderer>();
        }

        public void Set(MinoType type) {
            var mino = resolver.Get(type);
            var minoRenderer = mino.GetComponent<SpriteRenderer>();
            container.sprite = minoRenderer.sprite;
        }
    }
}
