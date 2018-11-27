using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoFrame : MonoBehaviour {
        [SerializeField]
        private BuyoResolver resolver;
        [SerializeField]
        private SpriteRenderer container;

        private void Reset() {
            container = GetComponentInChildren<SpriteRenderer>();
        }

        public void Set(List<BuyoType> types) {
            foreach (BuyoType type in types) {
                var mino = resolver.Get(type);
                var minoRenderer = mino.GetComponent<SpriteRenderer>();
                container.sprite = minoRenderer.sprite;
            }
        }
    }
}
