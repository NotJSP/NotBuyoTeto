using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [RequireComponent(typeof(HoldBuyoView))]
    public class HoldBuyo : MonoBehaviour {
        [SerializeField]
        private HoldBuyoView view;

        public Tuple<BuyoType, BuyoType> Types { get; private set; }
        public bool Locked { get; private set; }

        private void Reset() {
            view = GetComponent<HoldBuyoView>();
        }

        public void Lock() {
            Locked = true;
        }

        public void Free() {
            Locked = false;
        }

        public virtual void Clear() {
            Types = null;
            view.Clear();
            Free();
        }

        public virtual void Set(Tuple<BuyoType, BuyoType> types) {
            Types = types;
            Lock();
            view.Set(types);
        }
    }
}