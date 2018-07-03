﻿using System.Collections;
using UnityEngine;

namespace NotTetrin.Ingame.MultiPlay {
    public class NetworkInstantiator : Instantiator {
        public override GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, byte group) => PhotonNetwork.Instantiate(original.name, position, rotation, group);

        public override void Destroy(GameObject obj) {
            if (obj == null) { return; }
            
            var view = obj.GetComponent<PhotonView>();
            if (view) {
                PhotonNetwork.Destroy(view);
            } else {
                Object.Destroy(obj);
            }
        }

        public override void Destroy(GameObject obj, float t) => StartCoroutine(destroyAfterSeconds(obj, t));

        private IEnumerator destroyAfterSeconds(GameObject obj, float t) {
            yield return new WaitForSeconds(t);
            this.Destroy(obj);
        }
    }
}
