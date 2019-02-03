using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class TetoDirector : Director {
        [SerializeField]
        private TetoPerspective perspective;
        [SerializeField]
        private MinoManager minoManager;
        [SerializeField]
        private TetoGarbageManager garbageManager;

        private ColliderField colliderField => perspective.ColliderField;

        private static readonly float DefaultFallSpeed = 1.0f;

        public override bool IsGameOver => perspective.Field.Ceiling.IsHit;

        private void OnEnable() {
            minoManager.gameObject.SetActive(true);
            garbageManager.gameObject.SetActive(true);
        }

        private void OnDisable() {
            minoManager.gameObject.SetActive(false);
            garbageManager.gameObject.SetActive(false);
        }

        public override void Initialize() {
            minoManager.Initialize();
            minoManager.HitMino += onHitMino;
            colliderField.LineDeleted += onLineDeleted;
        }

        public override void RoundStart() {
            minoManager.Restart(DefaultFallSpeed);
            garbageManager.Clear();
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
            photonView.RPC("OnDeleteLineOpponent", PhotonTargets.Others, info.LineCount, info.ObjectCount);
        }

        private void onHitMino(object sender, EventArgs args) {
            minoManager.Release();

            // ゲームオーバー
            if (IsGameOver) {
                GameOver();
            } else {
                colliderField.DeleteLines();
                StartCoroutine(fallGarbageAndNext());
            }
        }

        private IEnumerator fallGarbageAndNext() {
            garbageManager.Fall();
            yield return new WaitWhile(() => garbageManager.IsFalling);
            Next();
        }
    }
}
