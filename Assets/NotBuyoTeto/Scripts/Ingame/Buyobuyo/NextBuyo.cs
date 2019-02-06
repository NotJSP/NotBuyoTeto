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
            //BuyoType.black,
        };

        public List<BuyoType[]> Types { get; private set; } = new List<BuyoType[]>();

        public void Awake() {
            
        }

        public void Clear() {
            Types.Clear();
        }

        public BuyoType[] Pop() {
            for (int i = Types.Count; i <= frames.Length; i++) {
                enqueueGroup();
            }
            var type = Types[0];

            Types.RemoveAt(0);
            updateView();
            return type;
        }

        private void enqueueGroup() {
            var buyo1 = allTypes[Random.Range(0, allTypes.Length)];
            var buyo2 = allTypes[Random.Range(0, allTypes.Length)];
            var group = new BuyoType[] { buyo1, buyo2 };
            Types.Add(group);
        }

        private void updateView() {
            for (int i = 0; i < frames.Length; i++) {
                var type = Types[i];
                frames[i].Set(type);
            }
        }
    }
}
