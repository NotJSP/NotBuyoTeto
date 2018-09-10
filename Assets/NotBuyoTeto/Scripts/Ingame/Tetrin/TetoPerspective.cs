using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class TetoPerspective : BuyoTetoPerspective {
        [SerializeField]
        private TetoField field;
        public TetoField Field => field;
        [SerializeField]
        private NextMino nextMino;
        public NextMino NextMino => nextMino;
        [SerializeField]
        private HoldMino holdMino;
        public HoldMino HoldMino => holdMino;

        public void OnRoundStarted() {
            field.Floor.SetActive(true);
            field.Ceiling.gameObject.SetActive(true);
        }

        public void OnRoundEnded() {
            field.Floor.SetActive(false);
            field.Ceiling.gameObject.SetActive(false);
        }
    }
}
