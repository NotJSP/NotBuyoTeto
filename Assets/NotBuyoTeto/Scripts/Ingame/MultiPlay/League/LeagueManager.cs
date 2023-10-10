using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Ingame.MultiPlay.Menu;
using NotBuyoTeto.Ingame.MultiPlay.Waiting;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NotBuyoTeto.Ingame.MultiPlay.League {
    public class LeagueManager : MonoBehaviourPunCallbacks {
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

        public override void OnEnable() {
            base.OnEnable();
            mainPanel.SetActive(true);
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
            PhotonNetwork.JoinLobby(LobbyManager.LeagueLobby);
        }

        public void OnCancel() {
            Debug.Log(@"LeagueManager::OnCancel");

            backButton.Inactive();

            if (PhotonNetwork.InRoom) {
                PhotonNetwork.LeaveRoom();
            }
            if (PhotonNetwork.InLobby) {
                PhotonNetwork.LeaveLobby();
            }

            OutMenu(() => {
                menuManager.gameObject.SetActive(true);
                menuManager.InMenu(() => backButton.Active());
                gameObject.SetActive(false);
            });
        }

        public override void OnJoinedLobby() {
            Debug.Log("LeagueManager::OnJoinedLobby");
            statusLabel.text = $"あなた: {PhotonNetwork.NickName}";
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom() {
            if (PhotonNetwork.PlayerListOthers.Length != 0) {
                onMatchingSucceeded();
            }
        }

        public override void OnPlayerEnteredRoom(Player player) {
            onMatchingSucceeded();
        }

        private void onMatchingSucceeded() {
            backButton.Inactive();
            StartCoroutine(AnimationTransit.Out(transit, () => {
                waitingManager.gameObject.SetActive(true);
                waitingManager.InMenu();
                gameObject.SetActive(false);
            }));
        }

        public override void OnLeftRoom() {
            Debug.Log("LeagueManager::OnLeftRoom");
        }

        public override void OnLeftLobby() {
            Debug.Log("LeagueManager::OnLeftLobby");
        }

        public override void OnJoinRandomFailed(short code, string msg) {
            Debug.Log("LeagueManager::OnPhotonRandomJoinFailed");

            var properties = new Hashtable();
            properties["wins"] = 2;         // Win数
            properties["speed"] = 1.0f;     // 落下速度
            properties["type"] = MatchingType.League;   // マッチングの種類

            var options = new RoomOptions {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = 2,
                CustomRoomProperties = properties,
                CustomRoomPropertiesForLobby = new string[] { "wins", "speed", "type" },
            };
            PhotonNetwork.CreateRoom("", options, LobbyManager.LeagueLobby);
        }

        public override void OnCreatedRoom() {
            Debug.Log("LeagueManager::OnCreatedRoom");
        }

        public override void OnCreateRoomFailed(short code, string msg) {
            Debug.Log("LeagueManager::OnPhotonCreateRoomFailed");
            statusLabel.text = @"ルーム作成に失敗しました";
        }
    }
}