using UnityEngine;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class BuyoDirector : MonoBehaviour {
        [SerializeField]
        private BuyoPerspective perspective;
        public BuyoPerspective Perspective => perspective;
    }
}
