using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.Tetrin;

namespace NotBuyoTeto.Ingame.SinglePlay.Tetrin {
    [RequireComponent(typeof(ColliderField))]
    public class LineDeleteScoring : MonoBehaviour {
        [SerializeField]
        private Score score;
        private ColliderField colliderField;

        private void Awake() {
            colliderField = GetComponent<ColliderField>();
        }

        private void Start() {
            colliderField.LineDeleted += onLineDeleted;
        }

        private void onLineDeleted(object sender, DeleteMinoInfo info) {
            var baseScore = 500.0f;
            var lines = info.LineCount;
            var amount = baseScore * Mathf.Pow(lines, 2) - baseScore * 0.15f * Mathf.Pow(lines, 2);
            score.Increase((int)amount);
        }
    }
}
