using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Mino : MonoBehaviour {
        [SerializeField]
        private MinoType type;
        public MinoType Type => type;
    }
}
