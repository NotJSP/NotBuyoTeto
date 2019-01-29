using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin {
    public class TetrinDirector : Director {
        [SerializeField]
        private TetoPerspective perspective;
        [SerializeField]
        private MinoManager minoManager;
        [SerializeField]
        private GarbageMinoManager garbageManager;
        [SerializeField]
        private GarbageTransfer garbageTransfer;

        private ColliderField colliderField => perspective.ColliderField;

        private static readonly float FallSpeed = 0.75f;

        public override bool IsGameOver => perspective.Field.Ceiling.IsHit;
        
        public override void Initialize() {
            minoManager.HitMino += onHitMino;
            colliderField.LineDeleted += onLineDeleted;
        }

        public override void ClearObjects() {
            minoManager.Initialize(FallSpeed);
            garbageManager.Clear();
        }

        public override void RoundEnd() {
            minoManager.Release();
        }

        public override void Next() {
            minoManager.Next();
        }

        public override void GameOver() {
            perspective.Field.Floor.SetActive(false);
        }

        private void onLineDeleted(object sender, DeleteMinoInfo info) {
            photonView.RPC("OnDeleteMinoOpponent", PhotonTargets.Others, info.LineCount, info.ObjectCount);
        }

        private void onHitMino(object sender, EventArgs args) {
            // minoManager.Release();

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

        [PunRPC]
        private void OnDeleteMinoOpponent(int lineCount, int objectCount) {
            var info = new DeleteMinoInfo(lineCount, objectCount);
            var garbageCount = garbageTransfer.GetGarbageCount(info, PlayerSideGameMode);
            garbageManager.Add(garbageCount);
        }
    }
}
