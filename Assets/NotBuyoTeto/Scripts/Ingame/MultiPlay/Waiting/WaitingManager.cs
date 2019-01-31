using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class WaitingManager : PunBehaviour {
        [SerializeField]
        private StartingCounter counter;

        [SerializeField]
        private GameObject waitingPanel;
        [SerializeField]
        private PlayerPanel playerPanel;
        [SerializeField]
        private OpponentPanel opponentPanel;
        [SerializeField]
        private GameObject waitingWindow;
        [SerializeField]
        private StartingCounter startingCounter;

        private AnimationTransitEntry playerPanelTransition;
        private AnimationTransitEntry opponentPanelTransition;
        private AnimationTransitEntry waitingWindowTransition;

        private void Awake() {
            playerPanelTransition = new AnimationTransitEntry(playerPanel.gameObject, "Panel In", "Panel Out");
            opponentPanelTransition = new AnimationTransitEntry(opponentPanel.gameObject, "Panel In", "Panel Out");
            waitingWindowTransition = new AnimationTransitEntry(waitingWindow, "In", "Out");
        }

        public void StartByHost(WaitingPlayer player, Action afterAction = null) {
            waitingPanel.SetActive(true);

            playerPanel.Set(player);
            StartCoroutine(AnimationTransit.In(playerPanelTransition));
            StartCoroutine(AnimationTransit.In(waitingWindowTransition));

            afterAction?.Invoke();
        }

        public void StartByGuest(WaitingPlayer player, WaitingPlayer opponent, Action afterAction = null) {
            waitingPanel.SetActive(true);

            playerPanel.Set(player);
            StartCoroutine(AnimationTransit.In(playerPanelTransition));
            opponentPanel.Set(opponent);
            StartCoroutine(AnimationTransit.In(opponentPanelTransition));

            startingCounter.OnZero += onCountZero;
            startingCounter.Set(30);
            startingCounter.CountStart();
            startingCounter.Show();

            afterAction?.Invoke();
        }

        private void onCountZero(object sender, EventArgs args) {
            PhotonNetwork.isMessageQueueRunning = false;
            SceneController.Instance.LoadScene(SceneName.NetworkBattle, SceneTransition.Duration);
        }

        public override void OnJoinedRoom() {
            Debug.Log("WaitingManager::OnJoinedRoom");
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer player) {
            Debug.Log("WaitingManager::OnPhotonPlayerConnected");
            // TODO:
            // var record = (FightRecord)player.CustomProperties["FightRecord"];
            // var rating = (int)player.CustomProperties["Rating"];
            var record = new FightRecord(1234, 768);
            var rating = 1523;
            var waitingPlayer = new WaitingPlayer(player.NickName, record, rating);
            opponentPanel.Set(waitingPlayer);
            StartCoroutine(AnimationTransit.Transition(waitingWindowTransition, opponentPanelTransition));

            PhotonNetwork.room.IsOpen = false;

            startingCounter.OnZero += onCountZero;
            startingCounter.Set(30);
            startingCounter.CountStart();
            startingCounter.Show();
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer player) {
            StartCoroutine(AnimationTransit.Transition(opponentPanelTransition, waitingWindowTransition));
            PhotonNetwork.room.IsOpen = true;
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
