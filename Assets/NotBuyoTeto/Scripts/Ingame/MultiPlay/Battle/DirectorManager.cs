using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.MultiPlay.Battle.Tetrin;
using NotBuyoTeto.Ingame.MultiPlay.Battle.BuyoBuyo;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class DirectorManager : MonoBehaviour {
        [SerializeField]
        private TetoDirector tetoDirector;
        [SerializeField]
        private BuyoDirector buyoDirector;

        private GameMode mode;

        public void SetMode(GameMode mode) {
            this.mode = mode;

            if (mode == GameMode.Tetrin) {
                tetoDirector.gameObject.SetActive(true);
                buyoDirector.gameObject.SetActive(false);
            }
            if (mode == GameMode.BuyoBuyo) {
                buyoDirector.gameObject.SetActive(true);
                tetoDirector.gameObject.SetActive(false);
            }
        }

        public Director GetDirector() {
            if (mode == GameMode.Tetrin) {
                return tetoDirector;
            }
            return buyoDirector;
        }
    }
}
