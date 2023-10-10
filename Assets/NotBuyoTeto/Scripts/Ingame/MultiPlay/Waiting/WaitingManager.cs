using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using NCMB;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;
using NotBuyoTeto.Ingame.MultiPlay.League;
using NotBuyoTeto.Ingame.MultiPlay.Club;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class WaitingManager : MonoBehaviourPunCallbacks {
        [SerializeField]
        private LeagueManager leagueManager;
        [SerializeField]
        private ClubManager clubManager;
        [SerializeField]
        private BackButton backButton;

        [SerializeField]
        private GameObject mainPanel;
        [SerializeField]
        private PlayerPanel playerPanel;
        [SerializeField]
        private OpponentPanel opponentPanel;
        [SerializeField]
        private GameObject waitingWindow;
        [SerializeField]
        private StartingCounter startingCounter;

        private MatchingType matchingType;
        private Coroutine startCoroutine;

        private AnimationTransitEntry playerPanelTransition;
        private AnimationTransitEntry opponentPanelTransition;
        private AnimationTransitEntry waitingWindowTransition;

        private bool isInPlayerPanel = false;
        private bool isInOpponentPanel = false;
        private bool isInWaitingWindow = false;

        private bool isDecidePlayerMode = false;
        private bool isDecideOpponentMode = false;

        private void Awake() {
            playerPanelTransition = new AnimationTransitEntry(playerPanel.gameObject, "Panel In", "Panel Out");
            opponentPanelTransition = new AnimationTransitEntry(opponentPanel.gameObject, "Panel In", "Panel Out");
            waitingWindowTransition = new AnimationTransitEntry(waitingWindow, "In", "Out");
        }

        public override void OnEnable() {
            base.OnEnable();
            mainPanel.SetActive(true);
            playerPanel.ModeContainerActivate(false);

            isDecidePlayerMode = false;
            isDecideOpponentMode = false;

            backButton.OnPressed += back;
        }

        public override void OnDisable() {
            base.OnDisable();
            mainPanel?.SetActive(false);
            backButton.OnPressed -= back;
        }

        private void Update() {
            if (AnimationTransit.IsAnimating) { return; }

            if (backButton.IsActive && Input.GetKeyDown(KeyCode.Escape)) {
                back(this, EventArgs.Empty);
            }
        }

        private bool readyToBattle => isInPlayerPanel && isInOpponentPanel;

        // 適当
        public IEnumerator OutMenu(Action afterAction = null) {
            if (isInPlayerPanel) { outPlayerPanel(); }
            if (isInOpponentPanel) { outOpponentPanel(); }
            if (isInWaitingWindow) { outWaitingWindow(); }
            yield return new WaitForSecondsRealtime(0.75f);
            mainPanel.SetActive(false);
            afterAction?.Invoke();
        }

        private void back(object sender, EventArgs args) {
            if (PhotonNetwork.InRoom) {
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

        public void InMenu(Action afterAction = null) {
            OnStart();
            afterAction?.Invoke();
        }

        public void OnStart() {
            matchingType = PhotonNetwork.CurrentLobby.Equals(LobbyManager.LeagueLobby) ? MatchingType.League : MatchingType.Club;

            var player = PhotonNetwork.LocalPlayer;
            var playerStats = getPlayerStatsFromProps(player);
            playerPanel.Set(new WaitingPlayer(player.NickName, playerStats));
            inPlayerPanel();

            // 既に他の人がいる場合
            if (PhotonNetwork.PlayerListOthers.Length > 0) {
                var opponent = PhotonNetwork.PlayerListOthers[0];
                var opponentStats = getPlayerStatsFromProps(opponent);
                opponentPanel.Set(new WaitingPlayer(opponent.NickName, opponentStats));
                inOpponentPanel();
                startModeSelect();
            } else {
                inWaitingWindow();
            }
        }

        private PlayerStats getPlayerStatsFromProps(Player player) {
            var props = player.CustomProperties;
            var rating = (int)props["rating"];
            var battleCount = (int)props["battleCount"];
            var winCount = (int)props["winCount"];
            return new PlayerStats(rating, battleCount, winCount);
        }

        private void startModeSelect() {
            isDecidePlayerMode = false;
            isDecideOpponentMode = false;

            playerPanel.ModeContainerActivate(true);

            startingCounter.OnZero += onCountZero;
            if (matchingType == MatchingType.Club) {
                startingCounter.Set(99);
                startingCounter.Hide();
            } else {
                startingCounter.Set(30);
                startingCounter.CountStart();
                startingCounter.Show();
            }
        }

        private void cancelModeSelect() {
            backButton.Active();

            cancelModeSelectTransition();
            playerPanel.ModeContainerActivate(false);

            startingCounter.OnZero -= onCountZero;
            startingCounter.Stop();
            startingCounter.Hide();

            if (startCoroutine != null) {
                StopCoroutine(startCoroutine);
                startCoroutine = null;
            }
        }

        private void onDecidePlayerMode(GameMode mode) {
            isDecidePlayerMode = true;
            if (isDecideOpponentMode) { onDecideBothMode(); }
        }

        private void onDecideOpponentMode(GameMode mode) {
            isDecideOpponentMode = true;
            if (isDecidePlayerMode) { onDecideBothMode(); }
        }

        private void onDecideBothMode() {
            if (startingCounter.Count > 3) {
                startingCounter.Set(3);
            }
            startingCounter.CountStart();
            startingCounter.Show();
        }

        private void onCountZero(object sender, EventArgs args) {
            startCoroutine = StartCoroutine(startBattle());
        }

        private IEnumerator startBattle() {
            if (!isDecidePlayerMode) {
                playerPanel.DecideRandomMode();
                playerPanel.DecideMode();
            }

            yield return new WaitUntil(() => isDecidePlayerMode);
            yield return new WaitUntil(() => isDecideOpponentMode);

            PhotonNetwork.IsMessageQueueRunning = false;
            yield return new WaitForSecondsRealtime(2.5f);

            SceneController.Instance.LoadScene(SceneName.NetworkBattle, SceneTransition.Duration);
        }

        public override void OnJoinedRoom() {
            Debug.Log("WaitingManager::OnJoinedRoom");
        }

        public override void OnPlayerPropertiesUpdate(Player player, Hashtable updatedProps) {
            Debug.Log("WaitingManager::OnPhotonPlayerPropertiesChanged");

            object value;
            if (updatedProps.TryGetValue("mode", out value)) {
                var mode = (GameMode)value;
                if (player.Equals(PhotonNetwork.LocalPlayer)) {
                    onDecidePlayerMode(mode);
                } else {
                    onDecideOpponentMode(mode);
                }
            }
        }

        public override void OnPlayerEnteredRoom(Player player) {
            Debug.Log("WaitingManager::OnPlayerEnteredRoom");
            // var record = (FightRecord)player.CustomProperties["FightRecord"];
            // var rating = (int)player.CustomProperties["Rating"];
            var stats = getPlayerStatsFromProps(player);
            opponentPanel.Set(new WaitingPlayer(player.NickName, stats));
            startModeSelectTransition();

            PhotonNetwork.CurrentRoom.IsOpen = false;
            startModeSelect();
        }

        public override void OnPlayerLeftRoom(Player player) {
            PhotonNetwork.CurrentRoom.IsOpen = true;
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
