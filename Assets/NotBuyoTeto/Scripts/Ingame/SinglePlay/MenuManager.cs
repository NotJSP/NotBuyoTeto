using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;
using NotBuyoTeto.UI;

namespace NotBuyoTeto.Ingame.SinglePlay {
    public class MenuManager : MonoBehaviour {
        [SerializeField]
        private SelectableToggler toggler;

        public void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                SceneController.Instance.LoadScene(SceneName.Title, SceneTransition.Duration);
            }
        }

        public void OnPressedTetrinButton() {
            toggler.ToggleAll();
            SceneController.Instance.LoadScene(SceneName.MarathonMode, SceneTransition.Duration);
        }

        public void OnPressedBuyoBuyoButton() {
            toggler.ToggleAll();
            SceneController.Instance.LoadScene(SceneName.TokotonMode, SceneTransition.Duration);
        }
    }
}