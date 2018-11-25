using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoSpawner : MonoBehaviour {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private BuyoResolver resolver;

        public GameObject Spawn(BuyoType type, Ceiling ceiling) {
            var spawnPosition = ceiling.transform.position;
            var buyo = resolver.Get(type);
            var obj = instantiator.Instantiate(buyo.gameObject, spawnPosition, Quaternion.identity);
            return obj;
        }
    }
}