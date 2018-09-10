using System;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Tetrin {
    [Serializable]
    public class TetoSfxTypeHolder<E> : IHolder<TetoSfxType, E> {
        public E MinoMove;
        public E MinoRotate;
        public E MinoHit;
        public E MinoHold;
        public E MinoDelete;

        public E this[TetoSfxType type] { 
            get {
                switch (type) {
                    case TetoSfxType.MinoMove:
                        return MinoMove;
                    case TetoSfxType.MinoRotate:
                        return MinoRotate;
                    case TetoSfxType.MinoHit:
                        return MinoHit;
                    case TetoSfxType.MinoHold:
                        return MinoHold;
                    case TetoSfxType.MinoDelete:
                        return MinoDelete;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 5;
    }
}
