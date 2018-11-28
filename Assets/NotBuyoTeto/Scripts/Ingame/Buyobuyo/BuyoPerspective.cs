using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoPerspective : BuyoTetoPerspective {
        [SerializeField]
        private BuyoField field;
        public BuyoField Field => field;
        [SerializeField]
        private NextBuyo nextBuyo;
        public NextBuyo NextBuyo => nextBuyo;

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
