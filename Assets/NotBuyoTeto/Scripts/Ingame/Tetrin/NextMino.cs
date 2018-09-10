using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.Tetrin {
    public class NextMino : MonoBehaviour {
        [SerializeField] private MinoFrame[] frames;
        [SerializeField] private MinoResolver resolver;

        private static MinoType[] allTypes = {
            MinoType.I,
            MinoType.O,
            MinoType.T,
            MinoType.L,
            MinoType.J,
            MinoType.S,
            MinoType.Z,
        };

        public List<MinoType> Types { get; private set; } = new List<MinoType>();

        public void Clear() {
            Types.Clear();
        }

        public MinoType Pop() {
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
