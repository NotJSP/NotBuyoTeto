using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoFrame : MonoBehaviour {
        [SerializeField]
        private BuyoResolver resolver;
        [SerializeField]
        private SpriteRenderer[] containers = new SpriteRenderer[2];
        
        private void Reset() {
            var containers = GetComponentsInChildren<SpriteRenderer>();
            this.containers[0] = containers[0];
            this.containers[1] = containers[1];
        }

        public virtual void Set(BuyoType[] types) {
            for (int i = 0; i < types.Length; i++) { 
                var buyo = resolver.Get(types[i]);
                var buyoRenderer = buyo.GetComponent<SpriteRenderer>();
                containers[i].sprite = buyoRenderer.sprite;
            }
        }
    }
}
