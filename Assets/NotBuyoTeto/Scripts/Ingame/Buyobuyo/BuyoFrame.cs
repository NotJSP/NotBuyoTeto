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

        public void Set(BuyoType type) {
            var buyo = resolver.Get(type);
            var buyoRenderer = buyo.GetComponent<SpriteRenderer>();
            container.sprite = buyoRenderer.sprite;
        }
    }
}
