using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Photon;
using NotBuyoTeto.SceneManagement;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class WaitingManager : PunBehaviour {
        [SerializeField]
        private MenuManager menuManager;
        [SerializeField]
        private StartingCounter counter;

        [SerializeField]
        private PlayerPanel playerPanel;
        [SerializeField]
        private PlayerPanel opponentPanel;
        [SerializeField]
        private GameObject waitingWindow;

        private AnimationTransitEntry playerPanelTransition;
        private AnimationTransitEntry opponentPanelTransition;
        private AnimationTransitEntry waitingWindowTransition;

        private void Awake() {
            playerPanelTransition = new AnimationTransitEntry(playerPanel.gameObject, "Panel In", "Panel Out");
            opponentPanelTransition = new AnimationTransitEntry(opponentPanel.gameObject, "Panel In", "Panel Out");
            waitingWindowTransition = new AnimationTransitEntry(waitingWindow, "In", "Out");
        }

        public void StartWaiting(WaitingPlayer player) {
            playerPanel.Set(player);
            AnimationTransit.In(playerPanelTransition);
            AnimationTransit.In(waitingWindowTransition);
        }

        private void back(object sender, EventArgs args) {
            menuManager.Back();
            counter.OnZero -= back;
        }

        public override void OnJoinedRoom() {
            Debug.Log("WaitingManager::OnJoinedRoom");
        }
    }
}
