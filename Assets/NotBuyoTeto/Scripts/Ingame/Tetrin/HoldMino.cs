using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    [RequireComponent(typeof(HoldMinoView))]
    public class HoldMino : MonoBehaviour {
        [SerializeField]
        private HoldMinoView view;

        public MinoType? Type { get; private set; }
        public bool Locked { get; private set; }

        private void Reset() {
            view = GetComponent<HoldMinoView>();
        }

        public void Lock() {
            Locked = true;
        }

        public void Free() {
            Locked = false;
        }

        public virtual void Clear() {
            Type = null;
            view.Clear();
            Free();
        }

        public virtual void Set(MinoType type) {
            Type = type;
            Lock();
            view.Set(type);
        }
    }
}