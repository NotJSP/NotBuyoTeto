using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Constants;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.SinglePlay.Tokoton {
    public class GameManager : SceneBase {
        [Header("References")]
        [SerializeField]
        private IngameSfxManager sfxManager;
        [SerializeField]
        private BuyoPerspective perspective;
        [SerializeField]
        private BuyoManager buyoManager;
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
            buyoManager.HitBuyo += onHitBuyo;
            buyoManager.DeleteBuyo += onDeleteBuyo;
            levelManager.ValueChanged += onLevelChanged;
            loadRanking();
            gameStart();
        }

        private void Update() {
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
            buyoManager.Restart(fallSpeedManager.DefaultSpeed);
            levelManager.Restart();
        }

        private void gameStart() {
            restart();
            perspective.OnGameStart();
            bgmManager.RandomPlay();
            sfxManager.Play(IngameSfxType.RoundStart);
            buyoManager.Next();
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
            var type = highScoreManager.RankingType;
            var userId = PlayerPrefs.GetString(PlayerPrefsKey.PlayerId);
            var score = highScoreManager.Value;
            var ranker = new Ranker(userId, score);
            rankingManager.Save(type, ranker);
        }

        private void onHitBuyo(object sender, EventArgs args) {
            buyoManager.Release();

            if (perspective.IsGameOver) {
                gameOver();
            } else {
                scoreManager.Increase(200 + (50 * levelManager.Value));
                buyoManager.Next();
            }
        }

        private void onLevelChanged(object sender, int level) {
            var fallSpeed = fallSpeedManager.GetSpeed(level);
            buyoManager.SetFallSpeed(fallSpeed);
            Debug.Log(fallSpeed);
        }

        private void onDeleteBuyo(object sender, DeleteBuyoInfo info) {
            levelManager.DeleteCountUp();
            int level = levelManager.Value;
            gameObject.GetComponent<BuyoDeleteScoring>().buyoDeleteScoring(level, info);
        }
    }
}