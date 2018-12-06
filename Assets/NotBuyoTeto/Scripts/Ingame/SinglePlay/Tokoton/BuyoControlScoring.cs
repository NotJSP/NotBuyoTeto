using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.SinglePlay.Tokoton {
    [RequireComponent(typeof(BuyoManager))]
    public class BuyoControlScoring : MonoBehaviour {
        [SerializeField]
        private Score score;
        private BuyoManager buyoManager;

        private static float ScoreIncrementDuration = 0.1f;

        private void Awake() {
            buyoManager = GetComponent<BuyoManager>();
        }

        private void Update() {
            var buyo = buyoManager.CurrentBuyo;
            if (buyo == null) { return; }

            var rigidbody = buyo.GetComponent<Rigidbody2D>();
            if (rigidbody == null) { return; }
            var controller = buyo.GetComponentInParent<Parent>();
            if (controller == null) { return; }

            var velocity = rigidbody.velocity;
            if (controller.DropTime % ScoreIncrementDuration == 1) {
                velocity.x = 0.0f;
                var amount = (int)(velocity.magnitude - 1.0f);
                score.Increase(amount);
            }
        }
    }
}
