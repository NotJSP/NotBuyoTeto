using System;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame {
    [Serializable]
    public class CommonSfxTypeHolder<E> : IHolder<CommonSfxType, E> {
        public E Decide;
        public E Cancel;
        public E Select;

        public E this[CommonSfxType type] {
            get {
                switch (type) {
                    case CommonSfxType.Decide:
                        return Decide;
                    case CommonSfxType.Cancel:
                        return Cancel;
                    case CommonSfxType.Select:
                        return Select;
                }
                throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public int Length => 3;
    }
}
