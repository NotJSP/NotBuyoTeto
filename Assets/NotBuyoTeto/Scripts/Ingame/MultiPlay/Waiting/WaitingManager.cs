using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using NotBuyoTeto.SceneManagement;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class WaitingManager : PunBehaviour {
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

        public override void OnJoinedRoom() {
            Debug.Log("WaitingManager::OnJoinedRoom");
        }

        public void OpenWaitingWindow() {
            StartCoroutine(AnimationTransit.In(waitingWindowTransition));
        }

        public void CloseWaitingWindow() {
            StartCoroutine(AnimationTransit.Out(waitingWindowTransition));
        }

        public void InPlayerPanel() {
            StartCoroutine(AnimationTransit.In(playerPanelTransition));
        }

        public void OutPlayerPanel() {
            StartCoroutine(AnimationTransit.Out(playerPanelTransition));
        }

        public void InOpponentPanel() {
            StartCoroutine(AnimationTransit.In(opponentPanelTransition));
        }

        public void OutOpponentPanel() {
            StartCoroutine(AnimationTransit.Out(opponentPanelTransition));
        }
    }
}
