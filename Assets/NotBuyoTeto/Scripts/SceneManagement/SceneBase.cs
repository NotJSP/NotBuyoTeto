using System;
using UnityEngine;
using NotBuyoTeto.Ingame;

namespace NotBuyoTeto.SceneManagement {
    public class SceneBase : MonoBehaviour {
        [SerializeField]
        protected GameTimer timer;
        [SerializeField]
        protected BgmManager bgmManager;

        public GameTimer Timer => timer;

        protected virtual void Start() {
            SceneController.Instance.SceneReady += OnSceneReady;
        }

        protected virtual void OnDestroy() {
            SceneController.Instance.SceneReady -= OnSceneReady;
        }

        protected virtual void OnSceneReady(object sender, EventArgs args) { }
    }
}
