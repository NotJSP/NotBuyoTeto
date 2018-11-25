using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Buyobuyo;

namespace NotBuyoTeto.Ingame.SinglePlay.Buyobuyo {
    [RequireComponent(typeof(BuyoManager))]
    public class BuyoControlScoring : MonoBehaviour {
        [SerializeField]
        private Score score;
        private BuyoManager buyoManager;

        private static int ScoreIncrementDuration = 5;

        private void Awake() {
            buyoManager = GetComponent<BuyoManager>();
        }

        private void Update() {
            var buyo = buyoManager.CurrentBuyo;
            if (buyo == null) { return; }

            var rigidbody = buyo.GetComponent<Rigidbody2D>();
            if (rigidbody == null) { return; }
            var controller = buyo.GetComponent<BuyoController>();
            if (controller == null) { return; }

            var velocity = rigidbody.velocity;
            if (controller.DropFrames % ScoreIncrementDuration == 1) {
                velocity.x = 0.0f;
                var amount = (int)(velocity.magnitude - 1.0f);
                score.Increase(amount);
            }
        }
    }
}
