using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    [RequireComponent(typeof(Button))]
    public class GameModeButton : MonoBehaviour, IPointerEnterHandler {
        [SerializeField]
        private GameMode gameMode;

        [Header("References")]
        [SerializeField]
        private PlayerPanel panel;
        [SerializeField]
        private GameModeButton pairButton;

        [Header("Appearance")]
        [SerializeField]
        private Color normalColor = Color.white;
        [SerializeField]
        private Color selectedColor = Color.white;
        [SerializeField]
        private Color decidedColor = Color.white;
        [SerializeField]
        private Color disabledColor = Color.white;
        
        [SerializeField]
        private float fadeDuration = 0.1f;

        private Button button;
        private Color fromColor = Color.white;
        private Color toColor = Color.white;
        private float fadeTime = 0.0f;
        private bool isFading = false;

        private void Awake() {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnPressedButton);
        }

        private void OnEnable() {
            button.interactable = true;
            button.image.color = normalColor;
            isFading = false;
        }

        private void Update() {
            if (!isFading) { return; }
            button.image.color = Color.Lerp(fromColor, toColor, fadeTime / fadeDuration);

            if (fadeTime >= fadeDuration) { isFading = false; }
            fadeTime += Time.fixedDeltaTime;
        }

        public void OnPointerEnter(PointerEventData data) {
            if (!button.interactable) { return; }
            panel.SelectMode(gameMode);
            pairButton.Unselect();
            fadeTo(selectedColor);
        }

        public void Unselect() {
            fromColor = button.image.color;
            fadeTo(normalColor);
        }

        public void OnPressedButton() {
            panel.DecideMode(gameMode);
            button.interactable = false;
            pairButton.Deactivate();
            fadeTo(decidedColor);
        }

        public void Deactivate() {
            button.interactable = false;
            fadeTo(disabledColor);
        }

        private void fadeTo(Color toColor) {
            this.fromColor = button.image.color;
            this.toColor = toColor;
            this.isFading = true;
            this.fadeTime = 0.0f;
        }
    }
}
