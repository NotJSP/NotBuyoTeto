using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class TetoDirector : Director {
        [SerializeField]
        private TetoPerspective perspective;
        [SerializeField]
        private MinoManager minoManager;
        [SerializeField]
        private TetoGarbageSpawner garbageSpawner;

        private ColliderField colliderField => perspective.ColliderField;

        private static readonly float DefaultFallSpeed = 1.0f;

        public override bool IsGameOver => perspective.Field.Ceiling.IsHit;

        private void OnEnable() {
            minoManager.gameObject.SetActive(true);
        }

        private void OnDisable() {
            minoManager.gameObject.SetActive(false);
        }

        public override void Initialize() {
            minoManager.Initialize();
            minoManager.HitMino += onHitMino;
            colliderField.LineDeleted += onLineDeleted;
        }

        public override void RoundStart() {
            minoManager.Restart(DefaultFallSpeed);
            garbageSpawner.Clear();
        }

        public override void RoundEnd() {
            minoManager.Release();
        }

        public override void Next() {
            minoManager.Next();
        }

        public override void GameStart() {
            perspective.Field.Floor.SetActive(true);
            base.GameStart();
        }

        public override void GameOver() {
            base.GameOver();
            perspective.Field.Floor.SetActive(false);
        }

        private void onLineDeleted(object sender, DeleteMinoInfo info) {
            var count = garbageCalculator.Calculate(info, gameManager.OpponentMode);
            var remain = garbageManager.Subtract(count);
            if (remain > 0) {
                garbageTransfer.Send(remain);
            }
        }

        private void onHitMino(object sender, EventArgs args) {
            minoManager.Release();

            // ゲームオーバー
            if (IsGameOver) {
                GameOver();
            } else {
                colliderField.DeleteLines();
                StartCoroutine(spawnGarbageAndNext());
            }
        }

        private IEnumerator spawnGarbageAndNext() {
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
