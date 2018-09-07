using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.SinglePlay.Marathon {
    [RequireComponent(typeof(GroupManager))]
    public class LineDeleteScoring : MonoBehaviour {
        [SerializeField]
        private Score score;
        private GroupManager groupManager;

        private void Awake() {
            groupManager = GetComponent<GroupManager>();
        }

        private void Start() {
            groupManager.MinoDeleted += onMinoDeleted;
        }

        private void onMinoDeleted(object sender, DeleteMinoInfo info) {
            var baseScore = 500.0f;
            var lines = info.LineCount;
            var amount = baseScore * Mathf.Pow(lines, 2) - baseScore * 0.15f * Mathf.Pow(lines, 2);
            score.Increase((int)amount);
        }
    }
}
