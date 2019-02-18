using System.Collections;
using UnityEngine;
using Photon;

namespace NotBuyoTeto.Network {
    public class NetworkConditionChecker : PunBehaviour {
        [SerializeField]
        private NetworkConditionIndicator indicator;

        public void Start() {
            StartCoroutine("startCheck");
        }

        private IEnumerator startCheck() {
            while (true) {
                if (PhotonNetwork.connected) {
                    var ping = PhotonNetwork.GetPing();
                    Debug.Log($"Ping: {ping}");
                    updateIndicator(ping);
                }
                yield return new WaitForSeconds(3.0f);
            }
        }

        private void updateIndicator(int ping) {
            var condition = getCondition(ping);
            indicator.Set(condition);
        }

        private NetworkConditionType getCondition(int ping) {
            if (ping < 60) {
                return NetworkConditionType.High;
            }
            if (ping < 180) {
                return NetworkConditionType.Middle;
            }
            return NetworkConditionType.Low;
        }
    }
}
