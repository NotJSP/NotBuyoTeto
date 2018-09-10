using System;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame {
    [Serializable]
    public class BuyoTetoTypeHolder<E> : IHolder<BuyoTetoType, E> {
        public E BuyoBuyo;
        public E Tetrin;

        public E this[BuyoTetoType type] {
            get {
                switch (type) {
                    case BuyoTetoType.BuyoBuyo: return BuyoBuyo;
                    case BuyoTetoType.Tetrin: return Tetrin;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 2;
    }
}