using System;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [Serializable]
    public class BuyoSfxTypeHolder<E> : IHolder<BuyoSfxType, E> {
        public E BuyoMove;
        public E BuyoRotate;
        public E BuyoHit;
        public E BuyoDelete;
        public E BuyoHold;

        public E this[BuyoSfxType type] { 
            get {
                switch (type) {
                    case BuyoSfxType.Move:
                        return BuyoMove;
                    case BuyoSfxType.Rotate:
                        return BuyoRotate;
                    case BuyoSfxType.Hit:
                        return BuyoHit;
                    case BuyoSfxType.Delete:
                        return BuyoDelete;
                    case BuyoSfxType.Hold:
                        return BuyoHold;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 5;
    }
}
