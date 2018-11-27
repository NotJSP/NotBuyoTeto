using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotBuyoTeto.SceneManagement;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class ConnectingPanel : MonoBehaviour {
        [SerializeField]
        private Text text;
        public Text Text => text;
        [SerializeField]
        public GameObject indicator;
        public GameObject Indicator => indicator;

        private AnimationTransitEntry transit;

        private void Reset() {
            this.text = GetComponentInChildren<Text>();
        }

        private void Awake() {
            this.transit = new AnimationTransitEntry(gameObject, "Panel In", "Panel Out");
        }

        public void Show() {
            AnimationTransit.In(transit);
        }

        public void Hide() {
            AnimationTransit.Out(transit);
        }
    }
}
