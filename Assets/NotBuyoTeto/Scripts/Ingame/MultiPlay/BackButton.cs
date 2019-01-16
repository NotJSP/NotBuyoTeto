using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotBuyoTeto.SceneManagement;

namespace NotBuyoTeto.Ingame.MultiPlay {
    [RequireComponent(typeof(Button))]
    public class BackButton : MonoBehaviour {
        private Button button;
        private AnimationTransitEntry transit;

        public bool IsActive => button.interactable;
        public event EventHandler OnPressed;

        private void Awake() {
            button = GetComponent<Button>();
            button.onClick.AddListener(onClick);
            transit = new AnimationTransitEntry(gameObject, "Back Button In", "Back Button Out");
        }

        private void onClick() {
            OnPressed?.Invoke(this, EventArgs.Empty);
        }
        
        public void Active() {
            button.interactable = true;
        }

        public void Inactive() {
            button.interactable = false;
        }

        public void In() {
            StartCoroutine(AnimationTransit.In(transit));
        }

        public void Out() {
            StartCoroutine(AnimationTransit.Out(transit));
        }
    }
}
