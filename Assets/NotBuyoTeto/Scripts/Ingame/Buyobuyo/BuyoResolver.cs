using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [Serializable]
    public class BuyoHolder : BuyoTypeHolder<Buyo> { }

    public class BuyoResolver : MonoBehaviour {
        [SerializeField]
        private BuyoHolder buyos;

        public int Length => buyos.Length;

        public Buyo Get(BuyoType type) => buyos[type];
    }
}