using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.SceneManagement {
    [RequireComponent(typeof(CanvasGroup), typeof(FadeImage))]
    public class LoadingScreen : MonoBehaviour {
        [SerializeField] private Slider slider;

        private CanvasGroup canvas;
        private FadeImage image;
        private AsyncOperation operation;

        public bool isFading { get; private set; }

        public void FadeIn(float interval) => StartCoroutine(fade(interval, 0f, 1f));
        public void FadeOut(float interval) => StartCoroutine(fade(interval, 1f, 0f));

        private void Awake() {
            canvas = GetComponent<CanvasGroup>();
            image = GetComponent<FadeImage>();
        }

        private IEnumerator fade(float interval, float from, float to) {
            isFading = true;

            var time = 0.0f;
            while (time <= interval) {
                var alpha = Mathf.Lerp(from, to, time / interval);
                image.Range = alpha;
                canvas.alpha = alpha;
                time += Time.deltaTime;
                yield return null;
            }

            isFading = false;
        }

        public void Play(AsyncOperation operation) {
            this.operation = operation;
        }

        public void Stop() {
            this.operation = null;
            slider.value = 0.0f;
        }

        public void Update() {
            if (operation == null) { return; }

            var progress = operation.progress / 0.9f;
            slider.value = progress;
        }
    }
}
