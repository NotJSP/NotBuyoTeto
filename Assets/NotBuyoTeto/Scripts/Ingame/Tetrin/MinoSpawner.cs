using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class MinoSpawner : MonoBehaviour {
        [SerializeField]
        private Instantiator instantiator;
        [SerializeField]
        private MinoResolver resolver;

        public GameObject Spawn(MinoType type, Vector3 position) {
            var mino = resolver.Get(type);
            return instantiator.Instantiate(mino.gameObject, position, Quaternion.identity);
        }
    }
}