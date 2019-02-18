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
                    case TetoSfxType.Move:
                        return MinoMove;
                    case TetoSfxType.Rotate:
                        return MinoRotate;
                    case TetoSfxType.Hit:
                        return MinoHit;
                    case TetoSfxType.Hold:
                        return MinoHold;
                    case TetoSfxType.Delete:
                        return MinoDelete;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 5;
    }
}
