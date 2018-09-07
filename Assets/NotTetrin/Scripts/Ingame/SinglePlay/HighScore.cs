using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotTetrin.Constants;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace NotTetrin.Ingame.SinglePlay {
    public class HighScore : MonoBehaviour {
        public RankingType RankingType;

        [SerializeField]
        private Text text;
        [SerializeField]
        private Score score;

        [SerializeField]
        private AnimationCurve animationCurve;
        [SerializeField]
        private float animationDuration;

        private static Vector2 START_SCALE = new Vector2(1.0f, 1.25f);
        private static Vector2 END_SCALE = new Vector2(1.0f, 1.0f);
        [SerializeField]
        private Gradient gradient;

        private Stopwatch animationStopwatch = new Stopwatch();
        private bool IsAnimating => animationStopwatch.IsRunning;

        private string playerPrefsKey => PlayerPrefsKey.HighScore[RankingType];

        public int Value {
            get {
                if (PlayerPrefs.HasKey(playerPrefsKey)) {
                    return PlayerPrefs.GetInt(playerPrefsKey);
                }
                return 0;
            }
            protected set {
                PlayerPrefs.SetInt(playerPrefsKey, value);
                updateText();
            }
        }

        private void Awake() {
            updateText();
        }

        private void Update() {
            if (IsAnimating) {
                var seconds = (float)animationStopwatch.Elapsed.TotalSeconds;
                if (seconds < animationDuration) {
                    var t = animationCurve.Evaluate(seconds / animationDuration);
                    text.rectTransform.localScale = Vector2.Lerp(START_SCALE, END_SCALE, t);
                    text.color = gradient.Evaluate(t);
                } else {
                    text.rectTransform.localScale = END_SCALE;
                    text.color = gradient.Evaluate(1.0f);
                    animationStopwatch.Reset();
                }
            }
        }

        public bool UpdateValue() {
            if (score.Value > Value) {
                Value = score.Value;
                animationStopwatch.Start();
                return true;
            }

            return false;
        }

        protected void updateText() {
            text.text = string.Format("{0:0000000}", Value);
        }
    }
}