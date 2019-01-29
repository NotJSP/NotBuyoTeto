using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public abstract class Director : PunBehaviour {
        public GameMode PlayerSideGameMode { get; private set; }
        public GameMode OpponentSideGameMode { get; private set; }

        public abstract bool IsGameOver { get; }

        public void SetMode(GameMode playerSideGameMode, GameMode opponentSideGameMode) {
            this.PlayerSideGameMode = playerSideGameMode;
            this.OpponentSideGameMode = opponentSideGameMode;
        }

        public abstract void Initialize();
        public abstract void ClearObjects();
        public abstract void RoundEnd();
        public abstract void Next();
        public abstract void GameOver();

        [PunRPC]
        private void OnDeleteBuyoOpponent(int objectCount, int comboCount) {

        }
    }
}
