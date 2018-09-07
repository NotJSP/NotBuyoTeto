using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotTetrin.Ingame {
    public class HoldMino : MonoBehaviour {
        [SerializeField] private Instantiator instantiator;
        [SerializeField] private IngameSfxManager sfxManager;
        [SerializeField] private MinoResolver resolver;
        [SerializeField] private Transform frame;

        private Animator animator;
        private GameObject mino;

        public int? Value { get; private set; }
        public bool Locked { get; private set; }

        private void Awake() {
            animator = GetComponent<Animator>();
        }

        public void Lock() {
            Locked = true;
        }

        public void Free() {
            Locked = false;
        }

        public void Clear() {
            if (mino != null) {
                instantiator.Destroy(mino);
            }
            Value = null;
        }

        public bool Push(int index) {
            // ロックされていたら何もしない
            if (Locked) {
                return false;
            }

            // 今のホールドミノを削除
            Clear();

            // 生成
            mino = instantiator.Instantiate(resolver.Get(index), frame.position, Quaternion.identity);
            // エフェクトの削除?
            instantiator.Destroy(mino.transform.GetChild(0).gameObject);
            Value = index;

            // ホールドをロック
            Lock();

            // アニメーション
            animator.Play(@"HoldAnimation", 0, 0.0f);
            // SE
            sfxManager.Play(IngameSfxType.MinoHold);

            return true;
        }
    }
}