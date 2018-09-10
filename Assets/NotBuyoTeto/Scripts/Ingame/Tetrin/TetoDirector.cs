using UnityEngine;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class TetoDirector : MonoBehaviour {
        [SerializeField]
        private TetoPerspective perspective;
        public TetoPerspective Perspective => perspective;
    }
}
