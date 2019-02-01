using System;
using UnityEngine;

namespace NotBuyoTeto.SceneManagement {
    public class SceneBase : MonoBehaviour {
        protected virtual void Start() {
            SceneController.Instance.SceneReady += OnSceneReady;
        }

        protected virtual void OnDestroy() {
            SceneController.Instance.SceneReady -= OnSceneReady;
        }

        protected virtual void OnSceneReady(object sender, EventArgs args) { }
    }
}
