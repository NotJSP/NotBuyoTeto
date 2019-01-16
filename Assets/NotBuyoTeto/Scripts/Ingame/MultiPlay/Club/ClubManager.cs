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
                backButton.Inactive();
                OutMenu(() => {
                    menuManager.gameObject.SetActive(true);
                    menuManager.InMenu(() => backButton.Active());
                    gameObject.SetActive(false);
                });
            }
            if (state == State.CreateRoom) {
                backButton.Inactive();
                StartCoroutine(AnimationTransit.Transition(createRoomTransit, transit, () => backButton.Active()));
                state = State.Menu;
            }
        }

        public void PressedCreateRoomButtonOnMainPanel() {
            StartCoroutine(AnimationTransit.Transition(transit, createRoomTransit, () => backButton.Active()));
            state = State.CreateRoom;
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
        }

        public void CreateRoomOnLobby() {
            Debug.Log("ClubManager::CreateRoomOnLobby");
            StartCoroutine(AnimationTransit.Transition(transit, createRoomTransit));
        }

        public void CreateRoomOnPanel() {
            Debug.Log("ClubManager::CreateRoomOnPanel");
            var name = PhotonNetwork.playerName;
            // TODO:
            var settings = new RoomSettings {
                WinsCount = 2,
                FallSpeed = 1.5f,
            };
            roomManager.CreateRoom(name, settings);

            // TODO:
            var record = new FightRecord(999, 999);
            var player = new WaitingPlayer(name, record, 1009);
            waitingManager.gameObject.SetActive(true);
            StartCoroutine(AnimationTransit.Out(createRoomTransit, () => waitingManager.StartWaiting(player)));
        }

        public void OnCancelCreateRoom() {
            Debug.Log("ClubManager::OnCancelCreateRoom");
            StartCoroutine(AnimationTransit.Transition(createRoomTransit, transit));
        }

        public override void OnJoinedLobby() {
            Debug.Log("ClubManager::OnJoinedLobby");
        }
    }
}