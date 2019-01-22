using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoPerspective : BuyoTetoPerspective {
        [SerializeField]
        private NextBuyo nextBuyo;
        public NextBuyo NextBuyo => nextBuyo;
    }
}
