using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame {
    public class MinoSpawner : MonoBehaviour {
        [SerializeField] Instantiator instantiator;
        [SerializeField] Director director;
        [SerializeField] MinoResolver resolver;

        private Ceiling ceiling => director.Ceiling;
        private Vector3 spawnPosition => ceiling.transform.position;

        public GameObject Spawn(int index, float offset = 0) {
            var position = spawnPosition;
            position.x += offset;
            var obj = instantiator.Instantiate(resolver.Get(index), position, Quaternion.identity);
            return obj;
        }
    }
}