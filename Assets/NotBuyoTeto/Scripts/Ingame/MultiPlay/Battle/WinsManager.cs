using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class WinsManager : MonoBehaviour {
        [SerializeField] WinsCounter[] counters;

        public bool Finished => (WinIndex != null);

        public int? WinIndex {
            get {
                for (int i = 0; i < counters.Length; i++) {
                    if (counters[i].CountLimited) { return i; }
                }
                return null;
            }
        }
    }
}
