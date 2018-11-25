using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Buyobuyo {
    public class NextBuyo : MonoBehaviour {
        [SerializeField] private BuyoFrame[] frames;
        [SerializeField] private BuyoResolver resolver;

        private static BuyoType[] allTypes = {
            BuyoType.red,
            BuyoType.blue,
            BuyoType.green,
            BuyoType.yellow,
            BuyoType.purple,
            BuyoType.black,
        };

        public List<BuyoType> Types { get; private set; } = new List<BuyoType>();

        public void Clear() {
            Types.Clear();
        }

        public BuyoType Pop() {
            if (Types.Count < resolver.Length) {
                enqueueGroup();
            }

            var type = Types[0];
            Types.RemoveAt(0);

            updateView();

            return type;
        }

        private void enqueueGroup() {
            var group = allTypes.Shuffle();
            Types.AddRange(group);
        }

        private void updateView() {
            for (int i = 0; i < frames.Length; i++) {
                var type = Types[i];
                frames[i].Set(type);
            }
        }
    }
}
