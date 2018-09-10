using System.Collections;
using System.Collections.Generic;

namespace NotBuyoTeto.Ingame {
    public interface IBuyoTetoLogic {
        void StartRound();
        void EndRound();
        void ReceiveGarbage(BuyoTetoType type, int count);
    }
}