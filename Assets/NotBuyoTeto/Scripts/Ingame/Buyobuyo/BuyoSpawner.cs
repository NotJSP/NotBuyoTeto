using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoSpawner : MonoBehaviour {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private BuyoResolver resolver;

        public GameObject Spawn(BuyoType type, Vector3 position, int i) {
            position.y += 0.75f * i;
            var buyo = resolver.Get(type);
            return instantiator.Instantiate(buyo.gameObject, position, Quaternion.identity);
        }
    }
}