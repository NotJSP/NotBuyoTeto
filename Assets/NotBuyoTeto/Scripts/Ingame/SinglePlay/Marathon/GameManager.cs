using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Constants;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.SinglePlay.Marathon {
    public class GameManager : SceneBase {
        [SerializeField]
        private TetoDirector director;
        [SerializeField]
        private BgmManager bgmManager;
        [SerializeField]
        private IngameSfxManager sfxManager;
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
        [SerializeField]
        private ColliderField colliderField;

        private TetoPerspective perspective => director.Perspective;
        private TetoField field => perspective.Field;

        protected override void OnSceneReady(object sender, EventArgs args) {
            base.OnSceneReady(sender, args);
            colliderField.LineDeleted += onLineDeleted;
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

            sfxManager.Stop(IngameSfxType.RoundEnd);
            score.Initialize();
            minoManager.Initialize(fallSpeedManager.DefaultSpeed);
            levelManager.Initialize();
        }

        private void roundstart() {
            reset();
            perspective.OnRoundStarted();
            bgmManager.RandomPlay();
            sfxManager.Play(IngameSfxType.RoundStart);
            minoManager.Next();
        }

        private void roundend() {
            perspective.OnRoundEnded();
            bgmManager.Stop();
            sfxManager.Play(IngameSfxType.RoundEnd);
            
            var updated = highScore.UpdateValue();
            if (updated) {
                saveRanking();
            }
            Invoke("roundstart", 9.0f);
        }

        private void loadRanking() {
            ranking.Fetch(RankingType.MarathonMode);
        }

        private void saveRanking() {
            var name = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            var score = highScore.Value;
            var ranker = new Ranker(name, score);
            ranking.Save(RankingType.MarathonMode, ranker);
        }

        private void onHitMino(object sender, EventArgs args) {
            minoManager.Release();

            // 天井に当たったらゲームオーバー
            if (field.Ceiling.IsHit) {
                roundend();
            } else {
                score.Increase(200 + (50 * levelManager.Value));
                field.ColliderField.DeleteLine();
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