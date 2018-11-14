using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;
using NotBuyoTeto.UI;

namespace NotBuyoTeto.Ingame.SinglePlay {
    public class TetrinButton : MonoBehaviour {
        [SerializeField]
        private SelectableToggler toggler;

        public void OnPressed() {
            toggler.ToggleAll();
            SceneController.Instance.LoadScene(SceneName.MarathonMode, SceneTransition.Duration);
        }
    }
}
