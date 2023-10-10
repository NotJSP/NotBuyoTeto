using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Constants;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.SinglePlay.Marathon {
    public class GameManager : SceneBase {
        [SerializeField]
        private IngameSfxManager sfxManager;
        [SerializeField]
        private TetoPerspective perspective;
        [SerializeField]
        private ColliderField colliderField;
        [SerializeField]
        private MinoManager minoManager;
        [SerializeField]
        private ScoreManager scoreManager;
        [SerializeField]
        private HighScoreManager highScoreManager;
        [SerializeField]
        private RankingManager rankingManager;
        [SerializeField]
        private LevelManager levelManager;
        [SerializeField]
        private FallSpeedManager fallSpeedManager;

        protected override void OnSceneReady(object sender, EventArgs args) {
            base.OnSceneReady(sender, args);
            colliderField.LineDeleted += onLineDeleted;
            minoManager.HitMino += onHitMino;
            minoManager.Initialize();
            levelManager.ValueChanged += onLevelChanged;
            loadRanking();
            gameStart();
        }

        public void Update() {
            if (Input.GetButtonDown(@"Escape")) {
                SceneController.Instance.LoadScene(SceneName.SinglePlay, SceneTransition.Duration);
            }
            if (Input.GetKeyDown(KeyCode.F12)) {
                gameStart();
            }
        }

        private void restart() {
            CancelInvoke("gameStart");

            sfxManager.Stop(IngameSfxType.GameOver);
            scoreManager.Restart();
            minoManager.Restart(fallSpeedManager.DefaultSpeed);
            levelManager.Restart();
        }

        private void gameStart() {
            restart();
            perspective.OnGameStart();
            bgmManager.RandomPlay();
            sfxManager.Play(IngameSfxType.RoundStart);
            minoManager.Next();
        }

        private void gameOver() {
            perspective.OnGameOver();
            bgmManager.Stop();
            sfxManager.Play(IngameSfxType.GameOver);
            
            var updated = highScoreManager.UpdateValue();
            if (updated) {
                saveRanking();
            }
            Invoke("gameStart", 9.0f);
        }

        private void loadRanking() {
            var type = highScoreManager.RankingType;
            var score = highScoreManager.Value;
            rankingManager.Fetch(type, score);
        }

        private void saveRanking() {
            var userId = PlayerPrefs.GetString(PlayerPrefsKey.PlayerId);
            var score = highScoreManager.Value;
            var ranker = new Ranker(userId, score);
            rankingManager.Save(highScoreManager.RankingType, ranker);
        }

        private void onHitMino(object sender, EventArgs args) {
            minoManager.Release();

            if (perspective.IsGameOver) {
                gameOver();
            } else {
                var score = 200 + (50 * levelManager.Value);
                scoreManager.Increase(score);
                colliderField.DeleteLines();
                minoManager.Next();
            }
        }

        private void onLineDeleted(object sender, DeleteMinoInfo info) {
            levelManager.CountUp(info.LineCount);
        }

        private void onLevelChanged(object sender, int level) {
            var fallSpeed = fallSpeedManager.GetSpeed(level);
            minoManager.SetFallSpeed(fallSpeed);
            Debug.Log(fallSpeed);
        }
    }
}