using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;
using NotBuyoTeto.Ingame.MultiPlay.League;
using NotBuyoTeto.Ingame.MultiPlay.Club;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class WaitingManager : PunBehaviour {
        [SerializeField]
        private LeagueManager leagueManager;
        [SerializeField]
        private ClubManager clubManager;
        [SerializeField]
        private BackButton backButton;

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

        private MatchingType matchingType;
        
        private AnimationTransitEntry playerPanelTransition;
        private AnimationTransitEntry opponentPanelTransition;
        private AnimationTransitEntry waitingWindowTransition;

        private bool isInPlayerPanel = false;
        private bool isInOpponentPanel = false;
        private bool isInWaitingWindow = false;

        private bool decidePlayerMode = false;
        private bool decideOpponentMode = false;

        private void Awake() {
            playerPanelTransition = new AnimationTransitEntry(playerPanel.gameObject, "Panel In", "Panel Out");
            opponentPanelTransition = new AnimationTransitEntry(opponentPanel.gameObject, "Panel In", "Panel Out");
            waitingWindowTransition = new AnimationTransitEntry(waitingWindow, "In", "Out");
        }

        private void OnEnable() {
            decidePlayerMode = false;
            decideOpponentMode = false;
            playerPanel.ModeContainerActivate(false);

            playerPanel.OnDecideMode += onDecidePlayerMode;
            opponentPanel.OnDecideMode += onDecideOpponentMode;

            backButton.OnPressed += back;
        }

        private void OnDisable() {
            playerPanel.OnDecideMode -= onDecidePlayerMode;
            opponentPanel.OnDecideMode -= onDecideOpponentMode;

            backButton.OnPressed -= back;
        }

        private void Update() {
            if (AnimationTransit.IsAnimating) { return; }

            if (backButton.IsActive && Input.GetKeyDown(KeyCode.Escape)) {
                back(this, EventArgs.Empty);
            }
        }

        // 適当
        public IEnumerator OutMenu(Action afterAction = null) {
            if (isInPlayerPanel) { outPlayerPanel(); }
            if (isInOpponentPanel) { outOpponentPanel(); }
            if (isInWaitingWindow) { outWaitingWindow(); }
            yield return new WaitForSecondsRealtime(0.75f);
            afterAction?.Invoke();
        }

        private void back(object sender, EventArgs args) {
            if (PhotonNetwork.inRoom) {
                PhotonNetwork.LeaveRoom();
            }

            backButton.Inactive();

            if (matchingType == MatchingType.League) {
                StartCoroutine(OutMenu(() => {
                    leagueManager.gameObject.SetActive(true);
                    leagueManager.InMenu(() => backButton.Active());
                    gameObject.SetActive(false);
                }));
            }

            if (matchingType == MatchingType.Club) {
                StartCoroutine(OutMenu(() => {
                    clubManager.gameObject.SetActive(true);
                    clubManager.InMenu(() => backButton.Active());
                    gameObject.SetActive(false);
                }));
            }
        }

        public void StartByHost(MatchingType type, WaitingPlayer player, Action afterAction = null) {
            matchingType = type;
            waitingPanel.SetActive(true);

            playerPanel.Set(player);
            inPlayerPanel();
            inWaitingWindow();

            afterAction?.Invoke();
        }

        public void StartByGuest(MatchingType type, WaitingPlayer player, WaitingPlayer opponent, Action afterAction = null) {
            matchingType = type;
            waitingPanel.SetActive(true);

            playerPanel.Set(player);
            opponentPanel.Set(opponent);
            inPlayerPanel();
            inOpponentPanel();

            afterAction?.Invoke();

            startModeSelect();
        }

        private void startModeSelect() {
            backButton.Inactive();

            decidePlayerMode = false;
            decideOpponentMode = false;

            startModeSelectTransition();
            playerPanel.ModeContainerActivate(true);

            startingCounter.OnZero += onCountZero;
            startingCounter.Set(30);
            startingCounter.CountStart();
            startingCounter.Show();
        }

        private void cancelModeSelect() {
            backButton.Active();

            cancelModeSelectTransition();
            playerPanel.ModeContainerActivate(false);

            startingCounter.OnZero -= onCountZero;
            startingCounter.Stop();
            startingCounter.Hide();
        }

        private void onDecidePlayerMode(object sender, GameMode mode) {
            decidePlayerMode = true;
            if (decideOpponentMode) { onDecideBothMode(); }
        }

        private void onDecideOpponentMode(object sender, GameMode mode) {
            decideOpponentMode = true;
            if (decidePlayerMode) { onDecideBothMode(); }
        }

        private void onDecideBothMode() {
            if (startingCounter.Count > 5) {
                startingCounter.Set(5);
            }
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
            var record = new FightRecord(0, 0);
            var rating = 1000;
            var waitingPlayer = new WaitingPlayer(player.NickName, record, rating);
            opponentPanel.Set(waitingPlayer);

            PhotonNetwork.room.IsOpen = false;
            startModeSelect();
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer player) {
            PhotonNetwork.room.IsOpen = true;
            cancelModeSelect();
        }

        private void inPlayerPanel() {
            isInPlayerPanel = true;
            StartCoroutine(AnimationTransit.In(playerPanelTransition));
        }

        private void outPlayerPanel() {
            isInPlayerPanel = false;
            StartCoroutine(AnimationTransit.Out(playerPanelTransition));
        }

        private void inOpponentPanel() {
            isInOpponentPanel = true;
            StartCoroutine(AnimationTransit.In(opponentPanelTransition));
        }

        private void outOpponentPanel() {
            isInOpponentPanel = false;
            StartCoroutine(AnimationTransit.Out(opponentPanelTransition));
        }

        private void inWaitingWindow() {
            isInWaitingWindow = true;
            StartCoroutine(AnimationTransit.In(waitingWindowTransition));
        }

        private void outWaitingWindow() {
            isInWaitingWindow = false;
            StartCoroutine(AnimationTransit.Out(waitingWindowTransition));
        }

        private void startModeSelectTransition() {
            isInWaitingWindow = false;
            isInOpponentPanel = true;
            StartCoroutine(AnimationTransit.Transition(waitingWindowTransition, opponentPanelTransition));
        }

        private void cancelModeSelectTransition() {
            isInOpponentPanel = false;
            isInWaitingWindow = true;
            StartCoroutine(AnimationTransit.Transition(opponentPanelTransition, waitingWindowTransition));
        }
    }
}
