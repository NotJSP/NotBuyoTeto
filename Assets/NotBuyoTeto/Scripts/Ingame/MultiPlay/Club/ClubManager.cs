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
        [SerializeField]
        private MenuManager menuManager;
        [SerializeField]
        private WaitingManager waitingManager;
        [SerializeField]
        private BackButton backButton;

        [SerializeField]
        private GameObject mainPanel;
        [SerializeField]
        private RoomManager roomManager;

        private enum State {
            Menu,
            CreateRoom,
        }
        private State state;
        private bool guest = false;

        private AnimationTransitEntry transit;
        private AnimationTransitEntry createRoomTransit;

        private void Awake() {
            this.transit = new AnimationTransitEntry(mainPanel, "In Menu", "Out Menu");
            this.createRoomTransit = new AnimationTransitEntry(mainPanel, "In CreateRoom", "Out CreateRoom");
        }

        private void OnEnable() {
            mainPanel.SetActive(true);
            backButton.OnPressed += back;
        }

        private void OnDisable() {
            mainPanel?.SetActive(false);
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
            state = State.Menu;
            StartCoroutine(AnimationTransit.In(transit, afterAction));
            OnStart();
        }

        public void OutMenu(Action afterAction = null) {
            StartCoroutine(AnimationTransit.Out(transit, afterAction));
        }

        public void OnStart() {
            Debug.Log("ClubManager::OnStart");
            if (!PhotonNetwork.insideLobby) { PhotonNetwork.JoinLobby(LobbyManager.ClubLobby); }
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
            StartCoroutine(AnimationTransit.Out(createRoomTransit, () => {
                waitingManager.gameObject.SetActive(true);
                waitingManager.InMenu(() => backButton.Active());
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
            OutMenu(() => {
                waitingManager.gameObject.SetActive(true);
                waitingManager.InMenu(() => backButton.Active());
                gameObject.SetActive(false);
            });
        }
    }
}