using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin {
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
        private static T instance;

        public static T Instance {
            get {
                if (instance) { return instance; }

                // Hierarchyから探す
                instance = FindObjectOfType<T>();
                if (instance) { return instance; }

                // Resourcesから探す
                var resource = Resources.Load<T>(typeof(T).Name);
                instance = Instantiate(resource);
                if (instance) { return instance; }

                return null;
            }
        }

        protected virtual void Awake() {
            if (instance != null && instance != this) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}