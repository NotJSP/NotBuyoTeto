using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace NotTetrin.SceneManagement {
    public class SceneController : SingletonMonoBehaviour<SceneController> {
        [SerializeField] Canvas canvas;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] LoadingScreen screen;

        public event EventHandler SceneReady;
        private string lastSceneName;
        private Coroutine coroutine;

        private void Start() {
            SceneReady?.Invoke(this, EventArgs.Empty);
        }

        public void LoadScene(string scene, float interval) {
            if (lastSceneName == scene) { return; }
            StartCoroutine(waitAndTransitScene(scene, interval));
            lastSceneName = scene;
        }

        private IEnumerator waitAndTransitScene(string scene, float interval) {
            yield return new WaitUntil(() => coroutine == null);
            coroutine = StartCoroutine(transitScene(scene, interval));
        }

        private IEnumerator transitScene(string scene, float interval) {
            // シーン読み込み
            var operation = SceneManager.LoadSceneAsync(scene);
            operation.allowSceneActivation = false;

            // フェードイン
            canvasGroup.blocksRaycasts = true;
            canvas.enabled = true;
            screen.FadeIn(interval);

            // ロードアニメーション
            screen.Play(operation);
            yield return new WaitWhile(() => screen.isFading);
            yield return new WaitWhile(() => operation.progress < 0.9f);

            // シーンをアクティブ化
            operation.allowSceneActivation = true;
            yield return new WaitUntil(() => operation.isDone);

            // シーン準備完了
            SceneReady?.Invoke(this, EventArgs.Empty);

            // フェードアウト
            screen.FadeOut(interval);
            yield return new WaitWhile(() => screen.isFading);

            // 後処理
            screen.Stop();
            canvas.enabled = false;
            canvasGroup.blocksRaycasts = false;

            // コルーチンの削除
            coroutine = null;
        }
    }
}