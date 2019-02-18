using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoPerspective : BuyoTetoPerspective {
        [SerializeField]
        private NextBuyo nextBuyo;
        public NextBuyo NextBuyo => nextBuyo;
    }
}
