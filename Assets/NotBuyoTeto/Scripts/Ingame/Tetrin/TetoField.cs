using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class TetoField : Field {
        [SerializeField]
        private ColliderField colliderField;
        public ColliderField ColliderField => colliderField;
    }
}
