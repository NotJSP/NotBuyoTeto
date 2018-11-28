using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class Buyo : MonoBehaviour {
        [SerializeField]
        private BuyoType type;
        public BuyoType Type => type;
    }
}
