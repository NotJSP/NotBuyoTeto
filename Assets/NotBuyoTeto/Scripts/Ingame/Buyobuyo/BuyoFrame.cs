using System;
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

        public virtual void Clear() {
            containers[0].sprite = null;
            containers[1].sprite = null;
        }

        public virtual void Set(Tuple<BuyoType, BuyoType> types) {
            set(0, types.Item2);
            set(1, types.Item1);
        }

        private void set(int index, BuyoType type) {
            var buyo = resolver.Get(type);
            var renderer = buyo.GetComponent<SpriteRenderer>();
            containers[index].sprite = renderer.sprite;
        }
    }
}
