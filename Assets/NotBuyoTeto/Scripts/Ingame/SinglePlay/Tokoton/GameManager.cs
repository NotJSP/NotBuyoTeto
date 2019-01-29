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
        private BgmManager bgmManager;
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
            levelManager.ValueChanged += onLevelChanged;
            loadRanking();
            roundStart();
        }

        private void Update() {
            if (Input.GetButtonDown(@"Escape")) {
                SceneController.Instance.LoadScene(SceneName.Title, 0.7f);
            }
            if (Input.GetKeyDown(KeyCode.F12)) {
                roundStart();
            }
        }

        private void reset() {
            CancelInvoke("roundStart");
            sfxManager.Stop(IngameSfxType.GameOver);
            scoreManager.Initialize();
            buyoManager.Initialize(fallSpeedManager.DefaultSpeed);
            levelManager.Initialize();
        }

        private void roundStart() {
            reset();
            perspective.OnRoundStart();
            bgmManager.RandomPlay();
            sfxManager.Play(IngameSfxType.RoundStart);
            buyoManager.Next();
        }

        private void gameover() {
            perspective.OnGameOver();
            bgmManager.Stop();
            sfxManager.Play(IngameSfxType.GameOver);
            
            var updated = highScoreManager.UpdateValue();
            if (updated) {
                saveRanking();
            }
            Invoke("roundStart", 9.0f);
        }

        private void loadRanking() {
            var type = highScoreManager.RankingType;
            var score = highScoreManager.Value;
            rankingManager.Fetch(type, score);
        }

        private void saveRanking() {
            var type = highScoreManager.RankingType;
            var name = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            var score = highScoreManager.Value;
            var ranker = new Ranker(name, score);
            rankingManager.Save(type, ranker);
        }

        private void onHitBuyo(object sender, EventArgs args) {
            buyoManager.Release();

            if (perspective.IsGameOver) {
                gameover();
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
    }
}