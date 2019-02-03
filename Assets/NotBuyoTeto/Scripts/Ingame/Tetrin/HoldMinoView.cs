using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    [RequireComponent(typeof(Animator))]
    public class HoldMinoView : MonoBehaviour {
        [SerializeField]
        private TetoSfxManager sfxManager;
        [SerializeField]
        private MinoFrame frame;

        private Animator animator;

        protected void Reset() {
            frame = GetComponentInChildren<MinoFrame>();
        }

        protected void Awake() {
            animator = GetComponent<Animator>();
        }

        public void Clear() {
            frame.Clear();
        }

        public void Set(MinoType type) {
            frame.Set(type);
            sfxManager.Play(TetoSfxType.MinoHold);
            animator.Play(@"HoldAnimation", 0, 0.0f);
        }
    }
}
