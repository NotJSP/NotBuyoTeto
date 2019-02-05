using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Ingame.MultiPlay.Menu;
using NotBuyoTeto.Ingame.MultiPlay.Waiting;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    public class ClubManager : PunBehaviour {
        private enum State {
            Menu,
            CreateRoom,
        }
        private State state;

        public static readonly TypedLobby Lobby = new TypedLobby("ClubLobby", LobbyType.Default);

        [SerializeField]
        private MenuManager menuManager;
        [SerializeField]
        private BackButton backButton;

        [SerializeField]
        private GameObject mainPanel;
        [SerializeField]
        private RoomManager roomManager;
        [SerializeField]
        private WaitingManager waitingManager;

        private AnimationTransitEntry transit;
        private AnimationTransitEntry createRoomTransit;
        private bool guest = false;

        private void Awake() {
            this.transit = new AnimationTransitEntry(mainPanel, "In Menu", "Out Menu");
            this.createRoomTransit = new AnimationTransitEntry(mainPanel, "In CreateRoom", "Out CreateRoom");
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

        private void back(object sender, EventArgs args) {
            if (state == State.Menu) {
                OnCancel();
            }
            if (state == State.CreateRoom) {
                OnCancelCreateRoom();
            }
        }

        public void InMenu(Action afterAction = null) {
            StartCoroutine(AnimationTransit.In(transit, afterAction));
        }

        public void OutMenu(Action afterAction = null) {
            StartCoroutine(AnimationTransit.Out(transit, afterAction));
        }

        public void OnStart() {
            Debug.Log("ClubManager::OnStart");
            PhotonNetwork.JoinLobby(Lobby);
            roomManager.Fetch();
        }

        public void OnCancel() {
            Debug.Log("ClubManager::OnCancel");

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

        public void OnCancelCreateRoom() {
            Debug.Log("ClubManager::OnCancelCreateRoom");
            backButton.Inactive();
            StartCoroutine(AnimationTransit.Transition(createRoomTransit, transit, () => backButton.Active()));
            state = State.Menu;
        }

        public override void OnJoinedLobby() {
            Debug.Log("ClubManager::OnJoinedLobby");
        }

        public void PressedCreateRoomOnLobby() {
            Debug.Log("ClubManager::PressedCreateRoomOnLobby");
            backButton.Inactive();
            StartCoroutine(AnimationTransit.Transition(transit, createRoomTransit, () => backButton.Active()));
            state = State.CreateRoom;
        }

        public void PressedCreateRoomOnPanel() {
            Debug.Log("ClubManager::PressedCreateRoomOnPanel");
            StartByHost();
        }

        public void StartByHost() {
            guest = false;

            // TODO:
            var settings = new RoomSettings {
                WinsCount = 2,
                FallSpeed = 1.5f,
            };
            roomManager.CreateRoom(settings);

            backButton.Inactive();
            waitingManager.gameObject.SetActive(true);
            StartCoroutine(AnimationTransit.Out(createRoomTransit, () => {
                // TODO:
                var record = new FightRecord(0, 0);
                var player = new WaitingPlayer(PhotonNetwork.playerName, record, 1000);
                waitingManager.StartByHost(player, () => backButton.Active());
                gameObject.SetActive(false);
            }));
        }

        public void StartByGuest(string roomName) {
            guest = true;
            PhotonNetwork.JoinRoom(roomName);
        }

        public override void OnJoinedRoom() {
            if (!guest) { return; }

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

                waitingManager.StartByGuest(player, opponent, () => backButton.Active());
                gameObject.SetActive(false);
            }));
        }
    }
}