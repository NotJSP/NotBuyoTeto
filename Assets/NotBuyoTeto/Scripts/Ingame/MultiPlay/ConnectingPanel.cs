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

        public bool IsShow { get; private set; }

        private void Reset() {
            this.text = GetComponentInChildren<Text>();
        }

        private void Awake() {
            this.transit = new AnimationTransitEntry(gameObject, "Panel In", "Panel Out");
        }

        public void Show() {
            gameObject.SetActive(true);
            IsShow = true;
            StartCoroutine(AnimationTransit.In(transit));
        }

        public void Hide() {
            IsShow = false;
            StartCoroutine(AnimationTransit.Out(transit, () => gameObject.SetActive(false)));
        }
    }
}
