using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.SinglePlay.Tetrin {
    [RequireComponent(typeof(MinoManager))]
    public class MinoControlScoring : MonoBehaviour {
        [SerializeField]
        private Score score;
        private MinoManager minoManager;

        private static int ScoreIncrementDuration = 5;

        private void Awake() {
            minoManager = GetComponent<MinoManager>();
        }

        private void Update() {
            var mino = minoManager.CurrentMino;
            if (mino == null) { return; }

            var rigidbody = mino.GetComponent<Rigidbody2D>();
            if (rigidbody == null) { return; }
            var controller = mino.GetComponent<MinoController>();
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
