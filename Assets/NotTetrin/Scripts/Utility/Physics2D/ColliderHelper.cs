using UnityEngine;

namespace NotTetrin.Utility.Physics2D {
    [RequireComponent(typeof(Collider2D))]
    public class ColliderHelper : MonoBehaviour {
        public Collider2D Collider { get; private set; }
        public int EnterCount { get; private set; }
        public bool IsEntered => EnterCount > 0;

        private void Awake() {
            this.Collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            ++EnterCount;
        }

        private void OnTriggerExit2D(Collider2D collision) {
            --EnterCount;
        }
    }
}