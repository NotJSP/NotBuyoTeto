using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.UI;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;

namespace NotTetrin.Ingame.Title {
    public class NetworkBattleButton : MonoBehaviour {
        [SerializeField]
        private SelectableToggler toggler;

        public void OnPressed() {
            toggler.ToggleAll();
            SceneController.Instance.LoadScene(SceneName.Matching, TitleManager.TransitionDuration);
        }
    }
}