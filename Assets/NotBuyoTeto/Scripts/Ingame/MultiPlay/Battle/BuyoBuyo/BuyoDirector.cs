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
        private BuyoGarbageManager garbageManager;
        [SerializeField]
        private GarbageTransfer garbageTransfer;

        private static readonly float DefaultFallSpeed = 1.0f;

        public override bool IsGameOver => perspective.Field.Ceiling.IsHit;

        private void OnEnable() {            
            buyoManager.gameObject.SetActive(true);
            garbageManager.gameObject.SetActive(true);
        }

        private void OnDisable() {
            buyoManager.gameObject.SetActive(false);
            garbageManager.gameObject.SetActive(false);
        }

        public override void Initialize() {
            buyoManager.HitBuyo += onHitBuyo;
            buyoManager.DeleteBuyo += onDeleteBuyo;
        }

        public override void RoundStart() {
            buyoManager.Restart(DefaultFallSpeed);
            garbageManager.Clear();
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
            photonView.RPC("OnDeleteBuyoOpponent", PhotonTargets.Others, info.ObjectCount, info.ComboCount);
        }

        private IEnumerator fallGarbageAndNext() {
            garbageManager.Fall();
            yield return new WaitWhile(() => garbageManager.IsFalling);
            Next();
        }
    }
}
