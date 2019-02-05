using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo {
    public class BuyoDirector : Director {
        [SerializeField]
        private BuyoPerspective perspective;
        [SerializeField]
        private BuyoManager buyoManager;
        [SerializeField]
        private BuyoGarbageSpawner garbageSpawner;

        private static readonly float DefaultFallSpeed = 1.0f;

        public override bool IsGameOver => perspective.Field.Ceiling.IsHit;

        private void OnEnable() {            
            buyoManager.gameObject.SetActive(true);
            garbageSpawner.gameObject.SetActive(true);
        }

        private void OnDisable() {
            buyoManager.gameObject.SetActive(false);
            garbageSpawner.gameObject.SetActive(false);
        }

        public override void Initialize() {
            buyoManager.HitBuyo += onHitBuyo;
            buyoManager.DeleteBuyo += onDeleteBuyo;
        }

        public override void RoundStart() {
            buyoManager.Restart(DefaultFallSpeed);
            garbageSpawner.Clear();
        }

        public override void RoundEnd() {
            buyoManager.Destroy();
        }

        public override void Next() {
            buyoManager.Next();
        }

        public override void GameStart() {
            perspective.OnGameStart();
            base.GameStart();
        }

        public override void GameOver() {
            base.GameOver();
            perspective.OnGameOver();
        }

        private void onHitBuyo(object sender, EventArgs args) {
            buyoManager.Release();

            // ゲームオーバー
            if (IsGameOver) {
                GameOver();
            } else {
                StartCoroutine(fallGarbageAndNext());
            }
        }

        private void onDeleteBuyo(object sender, DeleteBuyoInfo info) {
            var count = garbageCalculator.Calculate(info, gameManager.OpponentMode);
            var remain = garbageManager.Subtract(count);
            if (remain > 0) {
                garbageTransfer.Send(remain);
            }
        }

        private IEnumerator fallGarbageAndNext() {
            if (garbageManager.ReadyGarbageCount > 0) {
                var spawnCount = Math.Min(garbageManager.ReadyGarbageCount, 10);
                garbageSpawner.Spawn(spawnCount);
                garbageManager.Subtract(spawnCount);
                yield return new WaitWhile(() => garbageSpawner.IsSpawning);
            }
            Next();
        }
    }
}
