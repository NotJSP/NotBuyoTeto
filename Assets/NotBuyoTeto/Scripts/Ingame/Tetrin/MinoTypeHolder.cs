using System;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Tetrin {
    [Serializable]
    public class MinoTypeHolder<E> : IHolder<MinoType, E> {
        public E I = default(E);
        public E O = default(E);
        public E T = default(E);
        public E L = default(E);
        public E J = default(E);
        public E S = default(E);
        public E Z = default(E);

        public E this[MinoType type] {
            get {
                switch (type) {
                    case MinoType.I: return I;
                    case MinoType.O: return O;
                    case MinoType.T: return T;
                    case MinoType.L: return L;
                    case MinoType.J: return J;
                    case MinoType.S: return S;
                    case MinoType.Z: return Z;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 7;
    }
}
