//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace NotBuyoTeto.Ingame.Buyobuyo {
//    public class HoldBuyo : MonoBehaviour {
//        [SerializeField] private BuyoSfxManager sfxManager;
//        [SerializeField] private BuyoResolver resolver;
//        [SerializeField] private BuyoFrame frame;

//        private Animator animator;

//        public List<BuyoType>? Type { get; private set; }
//        public bool Locked { get; private set; }

//        private void Awake() {
//            animator = GetComponent<Animator>();
//        }

//        public void Lock() {
//            Locked = true;
//        }

//        public void Free() {
//            Locked = false;
//        }

//        public void Clear() {
//            Type = null;
//        }

//        public bool Push(List<BuyoType> type) {
//            // ロックされていたら何もしない
//            if (Locked) {
//                return false;
//            }

//            // セット
//            Type = type;
//            frame.Set(type);

//            // ホールドをロック
//            Lock();

//            // SE
//            sfxManager.Play(BuyoSfxType.BuyoHold);

//            // アニメーション
//            animator.Play(@"HoldAnimation", 0, 0.0f);

//            return true;
//        }
//    }
//}