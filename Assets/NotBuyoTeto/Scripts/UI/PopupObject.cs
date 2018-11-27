using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NotBuyoTeto.UI {
    public class PopupObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField]
        private Vector2 popupScale = new Vector2(1.0f, 1.0f);
        [SerializeField]
        private float transitTime = 0.5f;
        [SerializeField]
        private AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private Vector3 defaultScale;

        private float time = 0.0f;
        private bool entered = false;
        private bool isAnimating = false;

        private void Awake() {
            defaultScale = transform.localScale;
        }

        private void Update() {
            if (!isAnimating) { return; }

            time += Time.deltaTime;
            var progress = Mathf.Clamp01(time / transitTime);
            var t = animationCurve.Evaluate(progress);

            if (entered) {
                transform.localScale = Vector2.LerpUnclamped(defaultScale, popupScale, t);
            } else {
                transform.localScale = Vector2.LerpUnclamped(popupScale, defaultScale, t);
            }

            if (time >= transitTime) {
                isAnimating = false;
            }
        }

        public void OnPointerEnter(PointerEventData data) {
            time = 0.0f;
            entered = true;
            isAnimating = true;
        }

        public void OnPointerExit(PointerEventData data) {
            time = 0.0f;
            entered = false;
            isAnimating = true;
        }
    }
}