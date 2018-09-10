using System;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame {
    [Serializable]
    public class IngameSfxTypeHolder<E> : IHolder<IngameSfxType, E> {
        public E RoundStart;
        public E RoundEnd;

        public E this[IngameSfxType type] {
            get {
                switch (type) {
                    case IngameSfxType.RoundStart:
                        return RoundStart;
                    case IngameSfxType.RoundEnd:
                        return RoundEnd;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 2;
    }
}
