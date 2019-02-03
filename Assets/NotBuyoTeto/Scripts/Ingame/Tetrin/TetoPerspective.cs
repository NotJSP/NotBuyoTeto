using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class TetoPerspective : BuyoTetoPerspective {
        [SerializeField]
        private ColliderField colliderField;
        public ColliderField ColliderField => colliderField;
        [SerializeField]
        private NextMino nextMino;
        public NextMino NextMino => nextMino;
        [SerializeField]
        private HoldMino holdMino;
        public HoldMino HoldMino => holdMino;
    }
}
