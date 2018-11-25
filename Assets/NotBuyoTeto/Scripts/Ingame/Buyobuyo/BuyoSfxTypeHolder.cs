using System;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [Serializable]
    public class BuyoSfxTypeHolder<E> : IHolder<BuyoSfxType, E> {
        public E BuyoMove;
        public E BuyoRotate;
        public E BuyoHit;
        public E BuyoHold;
        public E BuyoDelete;

        public E this[BuyoSfxType type] { 
            get {
                switch (type) {
                    case BuyoSfxType.BuyoMove:
                        return BuyoMove;
                    case BuyoSfxType.BuyoRotate:
                        return BuyoRotate;
                    case BuyoSfxType.BuyoHit:
                        return BuyoHit;
                    case BuyoSfxType.BuyoHold:
                        return BuyoHold;
                    case BuyoSfxType.BuyoDelete:
                        return BuyoDelete;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 5;
    }
}
