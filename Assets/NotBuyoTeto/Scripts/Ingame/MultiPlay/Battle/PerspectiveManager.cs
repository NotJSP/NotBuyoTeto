using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class PerspectiveManager : MonoBehaviour {
        [Header("Player")]
        [SerializeField]
        public TetoPerspective PlayerTetoPerspective;
        [SerializeField]
        public BuyoPerspective PlayerBuyoPerspective;
        [Header("Opponent")]
        [SerializeField]
        public TetoPerspective OpponentTetoPerspective;
        [SerializeField]
        public BuyoPerspective OpponentBuyoPerspective;

        public void Activate(PlayerSide side, GameMode mode) {
            if (side == PlayerSide.Player) {
                if (mode == GameMode.Tetrin) {
                    PlayerTetoPerspective.gameObject.SetActive(true);
                }
                if (mode == GameMode.BuyoBuyo) {
                    PlayerBuyoPerspective.gameObject.SetActive(true);
                }
            }
            if (side == PlayerSide.Opponent) {
                if (mode == GameMode.Tetrin) {
                    OpponentTetoPerspective.gameObject.SetActive(true);
                }
                if (mode == GameMode.BuyoBuyo) {
                    OpponentBuyoPerspective.gameObject.SetActive(true);
                }
            }
        }
    }
}
