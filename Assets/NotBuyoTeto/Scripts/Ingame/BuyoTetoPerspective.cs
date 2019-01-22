using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame {
    public class BuyoTetoPerspective : MonoBehaviour, IBuyoTetoPerspective {
        [SerializeField]
        private Field field;
        public virtual Field Field => field;

        public virtual void OnRoundStart() {
            Field.Floor.SetActive(true);
            Field.Ceiling.Clear();
            Field.Ceiling.gameObject.SetActive(true);
        }

        public virtual void OnGameOver() {
            Field.Floor.SetActive(false);
            Field.Ceiling.gameObject.SetActive(false);
        }
    }
}
