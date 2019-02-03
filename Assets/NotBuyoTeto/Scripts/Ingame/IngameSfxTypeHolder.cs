using System;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame {
    [Serializable]
    public class IngameSfxTypeHolder<E> : IHolder<IngameSfxType, E> {
        public E RoundStart;
        public E GameOver;

        public E this[IngameSfxType type] {
            get {
                switch (type) {
                    case IngameSfxType.RoundStart:
                        return RoundStart;
                    case IngameSfxType.GameOver:
                        return GameOver;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 2;
    }
}
