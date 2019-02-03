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
            position.y += 0.75f * (i - 0.5f);
            var buyo = resolver.Get(type);
            var obj = instantiator.Instantiate(buyo.gameObject, position, Quaternion.identity);
            obj.GetComponent<Buyo>().Initialize(instantiator);
            return obj;
        }
    }
}