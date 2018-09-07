using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotTetrin.Ingame.MultiPlay {
    [RequireComponent(typeof(Animator))]
    public class WinsStar : MonoBehaviour {
        [SerializeField] SpriteRenderer star;

        private Animator animator;

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void Enable() {
            animator.Play("ToEnable", 0, 0.0f);
        }
    }
}
