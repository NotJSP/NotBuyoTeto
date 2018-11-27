using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;
using NotBuyoTeto.Ingame.MultiPlay.League;
using NotBuyoTeto.Ingame.MultiPlay.Club;
using Photon.Realtime;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class MenuManager : MonoBehaviourPunCallbacks {
        [SerializeField]
        private MatchingManager matchingManager;
        [SerializeField]
        private LobbyManager lobbyManager;

        [SerializeField]
        private GameObject menuPanel;
        [SerializeField]
        private GameObject leaguePanel;
        [SerializeField]
        private GameObject clubPanel;
        [SerializeField]
        private ConnectingPanel connectingPanel;

        private AnimationTransitEntry menuTransit;
        private AnimationTransitEntry leagueTransit;
        private AnimationTransitEntry clubTransit;
        private AnimationTransitEntry connectingTransit;

        private bool connected = false;

        private enum State {
            Menu,
            League,
            ClubMenu,
            CreateRoom,
        }

        private State state;

        private void Awake() {
            this.menuTransit = new AnimationTransitEntry(menuPanel, "Menu In", "Menu Out");
            this.leagueTransit = new AnimationTransitEntry(leaguePanel, "Open Window", "Close Window");
            this.clubTransit = new AnimationTransitEntry(clubPanel, "In Menu", "Out Menu");
            this.connectingTransit = new AnimationTransitEntry(connectingPanel.gameObject, "Panel In", "Panel Out");
        }

        private void Start() {
#if !DEBUG
            StartCoroutine(AnimationTransit.In(connectingTransit));
#endif
            PhotonNetwork.ConnectUsingSettings();
        }

        private void Update() {
            if (AnimationTransit.IsAnimating) { return; }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (state == State.Menu) {
                    if (PhotonNetwork.IsConnected) {
                        PhotonNetwork.Disconnect();
                    }
                    SceneController.Instance.LoadScene(SceneName.Title, SceneTransition.Duration);
                }

                if (state == State.League) {
                    CancelMatching();
                }

                if (state == State.ClubMenu) {
                    CancelClub();
                }

                if (state == State.CreateRoom) {
                    CancelCreateRoom();
                }
            }
        }

        public void OnPressedCreateRoomOnLobby() {
            state = State.CreateRoom;
            lobbyManager.CreateRoomOnLobby();
        }

        public void OnPressedCreateRoomOnPanel() {
            state = State.ClubMenu;
            lobbyManager.CreateRoomOnPanel();
        }

        public void OnPressedLeagueButton() {
            state = State.League;
            StartCoroutine(AnimationTransit.Transition(menuTransit, leagueTransit));
            matchingManager.StartMatching();
        }

        public void OnPressedClubButton() {
            state = State.ClubMenu;
            StartCoroutine(AnimationTransit.Transition(menuTransit, clubTransit));
        }

        public void CancelCreateRoom() {
            state = State.ClubMenu;
            lobbyManager.CancelCreateRoom();
        }

        public void CancelMatching() {
            state = State.Menu;
            StartCoroutine(AnimationTransit.Transition(leagueTransit, menuTransit));
            matchingManager.CancelMatching();
        }

        public void CancelClub() {
            state = State.Menu;
            StartCoroutine(AnimationTransit.Transition(clubTransit, menuTransit));
        }

        public override void OnConnectedToMaster() {
            if (!connected) {
                Debug.Log("OnConnectedToMaster");
                connected = true;
#if !DEBUG
                StartCoroutine(AnimationTransit.Out(connectingTransit));
#endif
            }
        }

        public override void OnDisconnected(DisconnectCause cause) {
            Debug.Log($"OnDisconnected(cause:{cause.ToString()})");
            if (connected) {
                Debug.Log("サーバーから切断されました。");
            } else {
                connectingPanel.Text.text = "インターネットに接続できません。\nタイトルに戻ります。";
                connectingPanel.indicator.SetActive(false);
            }
        }
    }
}