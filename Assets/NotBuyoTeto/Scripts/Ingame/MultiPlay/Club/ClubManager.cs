using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    public class ClubManager : PunBehaviour {
        public static readonly TypedLobby Lobby = new TypedLobby("ClubLobby", LobbyType.Default);

        [SerializeField]
        private GameObject mainPanel;
        [SerializeField]
        private RoomManager roomManager;

        private AnimationTransitEntry clubTransit;
        private AnimationTransitEntry createRoomTransit;

        private void Awake() {
            this.clubTransit = new AnimationTransitEntry(mainPanel, "In Menu", "Out Menu");
            this.createRoomTransit = new AnimationTransitEntry(mainPanel, "In CreateRoom", "Out CreateRoom");
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
            StartCoroutine(AnimationTransit.Transition(clubTransit, createRoomTransit));
        }

        public void CreateRoomOnPanel() {
            Debug.Log("ClubManager::CreateRoomOnPanel");
            StartCoroutine(AnimationTransit.Transition(createRoomTransit, clubTransit));
            var name = PhotonNetwork.playerName;
            // TODO:
            var settings = new RoomSettings {
                WinsCount = 2,
                FallSpeed = 1.5f,
            };
            roomManager.CreateRoom(name, settings);
        }

        public void OnCancelCreateRoom() {
            Debug.Log("ClubManager::OnCancelCreateRoom");
            StartCoroutine(AnimationTransit.Transition(createRoomTransit, clubTransit));
        }

        public override void OnJoinedLobby() {
            Debug.Log("ClubManager::OnJoinedLobby");
        }
    }
}