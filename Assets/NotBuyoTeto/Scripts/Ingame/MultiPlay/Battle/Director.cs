using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public abstract class Director : MonoBehaviour {
        [SerializeField]
        protected PhotonView photonView;

        public event EventHandler OnGameOver;

        public abstract bool IsGameOver { get; }

        public abstract void Initialize();
        public abstract void RoundStart();
        public abstract void RoundEnd();
        public abstract void Next();

        public virtual void GameStart() {
            Next();
        }

        public virtual void GameOver() {
            OnGameOver?.Invoke(this, EventArgs.Empty);
        }
    }
}
