using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [RequireComponent(typeof(Animator))]
    public class HoldBuyoView : MonoBehaviour {
        [SerializeField]
        private BuyoSfxManager sfxManager;
        [SerializeField]
        private BuyoFrame frame;

        private Animator animator;

        protected void Reset() {
            frame = GetComponentInChildren<BuyoFrame>();
        }

        protected void Awake() {
            animator = GetComponent<Animator>();
        }

        public void Clear() {
            frame.Clear();
        }

        public void Set(Tuple<BuyoType, BuyoType> types) {
            frame.Set(types);
            sfxManager.Play(BuyoSfxType.Hold);
            animator.Play(@"HoldAnimation", 0, 0.0f);
        }
    }
}
