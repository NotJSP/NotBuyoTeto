using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;

namespace NotBuyoTeto.Ingame.SinglePlay {
    public class MenuManager : MonoBehaviour {
        public void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                SceneController.Instance.LoadScene(SceneName.Title, SceneTransition.Duration);
            }
        }
    }
}