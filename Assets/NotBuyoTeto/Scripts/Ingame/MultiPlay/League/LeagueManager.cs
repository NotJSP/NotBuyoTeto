using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Ingame.MultiPlay.Menu;
using NotBuyoTeto.Ingame.MultiPlay.Waiting;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NotBuyoTeto.Ingame.MultiPlay.League {
    public class LeagueManager : PunBehaviour {
        public static readonly TypedLobby Lobby = new TypedLobby("LeagueLobby", LobbyType.Default);

        [SerializeField]
        private MenuManager menuManager;
        [SerializeField]
        private WaitingManager waitingManager;
        [SerializeField]
        private BackButton backButton;

        [SerializeField]
        private GameObject mainPanel;
        [SerializeField]
        private Text messageLabel;
        [SerializeField]
        private Text statusLabel;

        private AnimationTransitEntry transit;

        private void Awake() {
            transit = new AnimationTransitEntry(mainPanel, "Open Window", "Close Window");
        }

        private void OnEnable() {
            backButton.OnPressed += back;
        }

        private void OnDisable() {
            backButton.OnPressed -= back;
        }

        private void Update() {
            if (AnimationTransit.IsAnimating) { return; }

            if (backButton.IsActive && Input.GetKeyDown(KeyCode.Escape)) {
                back(this, EventArgs.Empty);
            }
        }

        public void InMenu(Action afterAction = null) {
            StartCoroutine(AnimationTransit.In(transit, afterAction));
            OnStart();
        }

        public void OutMenu(Action afterAction = null) {
            StartCoroutine(AnimationTransit.Out(transit, afterAction));
        }

        private void back(object sender, EventArgs args) {
            OnCancel();
        }

        public void OnStart() {
            Debug.Log(@"LeagueManager::OnStart");
            PhotonNetwork.JoinLobby(Lobby);
        }

        public void OnCancel() {
            Debug.Log(@"LeagueManager::OnCancel");

            if (PhotonNetwork.inRoom) {
                PhotonNetwork.LeaveRoom();
            }
            if (PhotonNetwork.connectionStateDetailed == ClientState.Authenticating || PhotonNetwork.connectionStateDetailed == ClientState.ConnectingToGameserver) { 
                PhotonNetwork.LeaveLobby();
            }

            backButton.Inactive();
            OutMenu(() => {
                menuManager.gameObject.SetActive(true);
                menuManager.InMenu(() => backButton.Active());
                gameObject.SetActive(false);
            });
        }

        public override void OnJoinedLobby() {
            Debug.Log("LeagueManager::OnJoinedLobby");
            statusLabel.text = $"あなた: {PhotonNetwork.playerName}";
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom() {
            if (PhotonNetwork.otherPlayers.Length != 0) {
                onMatchingSucceeded();
            }
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer player) {
            onMatchingSucceeded();
        }

        private void onMatchingSucceeded() {
            backButton.Inactive();
            waitingManager.gameObject.SetActive(true);

            StartCoroutine(AnimationTransit.Out(transit, () => {
                // TODO:
                var playerName = PhotonNetwork.playerName;
                var playerFightRecord = new FightRecord(0, 0);
                var player = new WaitingPlayer(playerName, playerFightRecord, 1000);

                var otherPlayer = PhotonNetwork.otherPlayers[0];
                var opponentName = otherPlayer.NickName;
                var opponentFightRecord = new FightRecord(0, 0);
                var opponent = new WaitingPlayer(opponentName, opponentFightRecord, 1000);

                waitingManager.StartByGuest(MatchingType.League, player, opponent);
                gameObject.SetActive(false);
            }));
        }

        public override void OnLeftRoom() {
            Debug.Log("LeagueManager::OnLeftRoom");
        }

        public override void OnLeftLobby() {
            Debug.Log("LeagueManager::OnLeftLobby");
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
            Debug.Log("LeagueManager::OnPhotonRandomJoinFailed");

            var properties = new Hashtable();
            properties["WinsCount"] = 2;
            properties["FallSpeed"] = 1.0f;

            var options = new RoomOptions {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = 2,
                CustomRoomProperties = properties,
                CustomRoomPropertiesForLobby = new string[] { "WinsCount", "FallSpeed" },
            };
            PhotonNetwork.CreateRoom("", options, Lobby);
        }

        public override void OnCreatedRoom() {
            Debug.Log("LeagueManager::OnCreatedRoom");
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
            Debug.Log("LeagueManager::OnPhotonCreateRoomFailed");
            statusLabel.text = @"ルーム作成に失敗しました";
        }
    }
}