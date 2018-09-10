using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    [Serializable]
    public class MinoHolder : MinoTypeHolder<Mino> { }

    public class MinoResolver : MonoBehaviour {
        [SerializeField]
        private MinoHolder minos;

        public int Length => minos.Length;

        public Mino Get(MinoType type) => minos[type];
    }
}