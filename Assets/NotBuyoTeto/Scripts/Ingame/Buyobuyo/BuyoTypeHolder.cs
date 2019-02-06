using System;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    [Serializable]
    public class BuyoTypeHolder<E> : IHolder<BuyoType, E> {
        public E red = default(E);
        public E blue = default(E);
        public E green = default(E);
        public E yellow = default(E);
        public E purple = default(E);
        //public E black = default(E);

        public E this[BuyoType type] {
            get {
                switch (type) {
                    case BuyoType.red: return red;
                    case BuyoType.blue: return blue;
                    case BuyoType.green: return green;
                    case BuyoType.yellow: return yellow;
                    case BuyoType.purple: return purple;
                    //case BuyoType.black: return black;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 5;
    }
}
