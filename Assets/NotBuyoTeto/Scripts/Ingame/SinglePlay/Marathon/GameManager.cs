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
        private BgmManager bgmManager;
        [SerializeField]
        private IngameSfxManager sfxManager;
        [SerializeField]
        private TetoPerspective perspective;
        [SerializeField]
        private MinoManager minoManager;
        [SerializeField]
        private Score score;
        [SerializeField]
        private HighScore highScore;
        [SerializeField]
        private Ranking ranking;
        [SerializeField]
        private LevelManager levelManager;
        [SerializeField]
        private FallSpeedManager fallSpeedManager;

        protected override void OnSceneReady(object sender, EventArgs args) {
            base.OnSceneReady(sender, args);
            perspective.ColliderField.LineDeleted += onLineDeleted;
            minoManager.HitMino += onHitMino;
            levelManager.ValueChanged += onLevelChanged;
            loadRanking();
            roundstart();
        }

        private void Update() {
            if (Input.GetButtonDown(@"Escape")) {
                SceneController.Instance.LoadScene(SceneName.Title, 0.7f);
            }
            if (Input.GetKeyDown(KeyCode.F12)) {
                roundstart();
            }
        }

        private void reset() {
            CancelInvoke("roundstart");

            sfxManager.Stop(IngameSfxType.GameOver);
            score.Initialize();
            minoManager.Initialize(fallSpeedManager.DefaultSpeed);
            levelManager.Initialize();
        }

        private void roundstart() {
            reset();
            perspective.OnRoundStart();
            bgmManager.RandomPlay();
            sfxManager.Play(IngameSfxType.RoundStart);
            minoManager.Next();
        }

        private void gameover() {
            perspective.OnGameOver();
            bgmManager.Stop();
            sfxManager.Play(IngameSfxType.GameOver);
            
            var updated = highScore.UpdateValue();
            if (updated) {
                saveRanking();
            }
            Invoke("roundstart", 9.0f);
        }

        private void loadRanking() {
            ranking.Fetch(highScore.RankingType);
        }

        private void saveRanking() {
            var name = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            var score = highScore.Value;
            var ranker = new Ranker(name, score);
            ranking.Save(highScore.RankingType, ranker);
        }

        private void onHitMino(object sender, EventArgs args) {
            minoManager.Release();

            // 天井に当たったらゲームオーバー
            if (perspective.Field.Ceiling.IsHit) {
                gameover();
            } else {
                score.Increase(200 + (50 * levelManager.Value));
                perspective.ColliderField.DeleteLine();
                minoManager.Next();
            }
        }

        private void onLineDeleted(object sende, DeleteMinoInfo info) {
            levelManager.DeleteCountUp(info.LineCount);
        }

        private void onLevelChanged(object sender, int level) {
            var fallSpeed = fallSpeedManager.GetSpeed(level);
            minoManager.SetFallSpeed(fallSpeed);
            Debug.Log(fallSpeed);
        }

    }
}