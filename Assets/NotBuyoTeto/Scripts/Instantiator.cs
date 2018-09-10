using UnityEngine;

namespace NotBuyoTeto {
    public abstract class Instantiator : MonoBehaviour {
        public abstract GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, byte group);
        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation) => Instantiate(original, position, rotation, 0);

        public abstract void Destroy(GameObject obj);
        public abstract void Destroy(GameObject obj, float t);
    }
}
