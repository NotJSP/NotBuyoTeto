using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class Director : MonoBehaviour {
        [SerializeField]
        private PerspectiveManager perspectives;

        public GameMode PlayerGameMode { get; private set; }
        public GameMode OpponentGameMode { get; private set; }

        public void SetMode(GameMode player, GameMode opponent) {
            this.PlayerGameMode = player;
            perspectives.Activate(PlayerSide.Player, PlayerGameMode);
            this.OpponentGameMode = opponent;
            perspectives.Activate(PlayerSide.Opponent, OpponentGameMode);
        }

        public Field PlayerField {
            get {
                if (PlayerGameMode == GameMode.Tetrin) {
                    return perspectives.PlayerTetoPerspective.Field;
                }
                return perspectives.PlayerBuyoPerspective.Field;
            }
        }

        public bool IsGameOver => PlayerField.Ceiling.IsHit;
    }
}
