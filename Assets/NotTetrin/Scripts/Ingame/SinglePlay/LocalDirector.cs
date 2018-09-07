using UnityEngine;

namespace NotTetrin.Ingame.SinglePlay {
    public class LocalDirector : Director {
        [SerializeField] GameObject floor;
        [SerializeField] Ceiling ceiling;
        [SerializeField] HoldMino holdMino;
        [SerializeField] NextMino nextMino;

        public override GameObject Floor => floor;
        public override Ceiling Ceiling => ceiling;
        public override HoldMino HoldMino => holdMino;
        public override NextMino NextMino => nextMino;
    }
}
