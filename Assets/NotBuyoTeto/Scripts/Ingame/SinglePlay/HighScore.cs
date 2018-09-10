using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NotBuyoTeto.Constants;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace NotBuyoTeto.Ingame.SinglePlay {
    public class HighScore : MonoBehaviour {
        private static Vector2 START_SCALE = new Vector2(1.0f, 1.25f);
        private static Vector2 END_SCALE = new Vector2(1.0f, 1.0f);

        [SerializeField]
        private RankingType rankingType;
        public RankingType RankingType => rankingType;

        [SerializeField]
        private Text text;
        [SerializeField]
        private Score score;

        [SerializeField]
        private AnimationCurve animationCurve;
        [SerializeField]
        private float animationDuration;

        [SerializeField]
        private Gradient gradient;

        private Stopwatch animationStopwatch = new Stopwatch();
        private bool isAnimating => animationStopwatch.IsRunning;

        private string prefsKey => PlayerPrefsKey.HighScore[rankingType];

        public int Value {
            get {
                if (PlayerPrefs.HasKey(prefsKey)) {
                    return PlayerPrefs.GetInt(prefsKey);
                }
                return 0;
            }
            protected set {
                PlayerPrefs.SetInt(prefsKey, value);
                updateText();
            }
        }

        private void Awake() {
            updateText();
        }

        private void Update() {
            if (isAnimating) {
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