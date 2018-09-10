using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class MinoSpawner : MonoBehaviour {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private MinoResolver resolver;

        public GameObject Spawn(MinoType type, Ceiling ceiling) {
            var spawnPosition = ceiling.transform.position;
            var mino = resolver.Get(type);
            var obj = instantiator.Instantiate(mino.gameObject, spawnPosition, Quaternion.identity);
            return obj;
        }
    }
}