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
        private SpriteRenderer[] containers;
        
        private void Reset() {
            var containers = GetComponentsInChildren<SpriteRenderer>();
            this.containers[0] = containers[0];
            this.containers[1] = containers[1];
        }

        public void Set(BuyoType[] types) {
            for(int i =0; i < types.Length; i++) { 
                var buyo = resolver.Get(types[i]);
                var buyoRenderer = buyo.GetComponent<SpriteRenderer>();
                containers[i].sprite = buyoRenderer.sprite;
            }
        }
    }
}
