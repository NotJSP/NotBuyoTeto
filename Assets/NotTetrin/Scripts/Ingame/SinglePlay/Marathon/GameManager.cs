using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;
using NotTetrin.Ingame.Marathon;
using NotTetrin.Utility.Tiling;

namespace NotTetrin.Ingame.SinglePlay.Marathon {
    public class GameManager : SceneBase {
        [SerializeField] Director director;
        [SerializeField] BGMManager bgmManager;
        [SerializeField] IngameSfxManager sfxManager;
        [SerializeField] MinoManager minoManager;
        [SerializeField] Score score;
        [SerializeField] HighScore highScore;
        [SerializeField] Ranking ranking;
        [SerializeField] GroupManager groupManager;
        [SerializeField] LevelManager levelManager;

        protected override void OnSceneReady(object sender, EventArgs args) {
            base.OnSceneReady(sender, args);
            minoManager.HitMino += onHitMino;
            loadRanking();
            gamestart();
        }

        private void Update() {
            if (Input.GetButtonDown(@"Escape")) {
                SceneController.Instance.LoadScene(SceneName.Title, 0.7f);
            }
            if (Input.GetKeyDown(KeyCode.F12)) {
                gamestart();
            }
        }

        private void reset() {
            CancelInvoke("gamestart");
            sfxManager.Stop(IngameSfxType.GameOver);
            score.Reset();
            minoManager.Reset();
            levelManager.Reset();
        }

        private void gamestart() {
            reset();
            director.Floor.SetActive(true);
            bgmManager.RandomPlay();
            sfxManager.Play(IngameSfxType.GameStart);
            minoManager.Next();
        }

        private void gameover() {
            director.Floor.SetActive(false);
            bgmManager.Stop();
            sfxManager.Play(IngameSfxType.GameOver);
            
            // TODO: 本番は常にセーブ
            var updated = highScore.UpdateValue();
            if (updated) {
                saveRanking();
            }
            Invoke("gamestart", 9.0f);
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
            if (director.Ceiling.IsHit) {
                gameover();
            } else {
                score.Increase(200 + (50 * levelManager.getLevel()));
                groupManager.DeleteMino();
                minoManager.Next();
            }
        }
    }
}