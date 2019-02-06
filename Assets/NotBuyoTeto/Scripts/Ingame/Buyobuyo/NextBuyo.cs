using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class NextBuyo : MonoBehaviour {
        [SerializeField]
        private BuyoFrame[] frames;
        [SerializeField]
        private BuyoResolver resolver;

        private static BuyoType[] allTypes = {
            BuyoType.red,
            BuyoType.blue,
            BuyoType.green,
            BuyoType.yellow,
            BuyoType.purple,
            //BuyoType.black,
        };

        public List<Tuple<BuyoType, BuyoType>> Types { get; private set; } = new List<Tuple<BuyoType, BuyoType>>();

        public void Clear() {
            Types.Clear();
        }

        public Tuple<BuyoType, BuyoType> Pop() {
            for (int i = Types.Count; i <= frames.Length; i++) {
                enqueueGroup();
            }
            var types = Types[0];

            Types.RemoveAt(0);
            updateView();
            return types;
        }

        private void enqueueGroup() {
            var buyo1 = allTypes[Random.Range(0, allTypes.Length)];
            var buyo2 = allTypes[Random.Range(0, allTypes.Length)];
            var tuple = new Tuple<BuyoType, BuyoType>(buyo1, buyo2);
            Types.Add(tuple);
        }

        private void updateView() {
            for (int i = 0; i < frames.Length; i++) {
                var types = Types[i];
                frames[i].Set(types);
            }
        }
    }
}
