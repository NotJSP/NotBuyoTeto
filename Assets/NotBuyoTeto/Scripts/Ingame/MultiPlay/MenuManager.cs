using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;
using NotBuyoTeto.Ingame.MultiPlay.League;
using NotBuyoTeto.Ingame.MultiPlay.Club;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class MenuManager : PunBehaviour {
        [SerializeField]
        private LeagueManager leagueManager;
        [SerializeField]
        private ClubManager clubManager;

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
            PhotonNetwork.ConnectUsingSettings("1.0");
        }

        private void Update() {
            if (AnimationTransit.IsAnimating) { return; }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                Back();
            }
        }

        public void Back() {
            if (state == State.Menu) {
                if (PhotonNetwork.connected) {
                    PhotonNetwork.Disconnect();
                }
                SceneController.Instance.LoadScene(SceneName.Title, SceneTransition.Duration);
            }

            if (state == State.League) {
                OnCancelLeague();
            }

            if (state == State.ClubMenu) {
                OnCancelClub();
            }

            if (state == State.CreateRoom) {
                OnCancelCreateRoom();
            }
        }

        public void OnPressedCreateRoomOnLobby() {
            state = State.CreateRoom;
            clubManager.CreateRoomOnLobby();
        }

        public void OnPressedCreateRoomOnPanel() {
            state = State.ClubMenu;
            clubManager.CreateRoomOnPanel();
        }

        public void OnPressedLeagueButton() {
            state = State.League;
            StartCoroutine(AnimationTransit.Transition(menuTransit, leagueTransit));
            //leagueManager.enabled = true;
            leagueManager.gameObject.SetActive(true);
            leagueManager.OnStart();
        }

        public void OnCancelLeague() {
            state = State.Menu;
            StartCoroutine(AnimationTransit.Transition(leagueTransit, menuTransit));
            leagueManager.OnCancel();
            //leagueManager.enabled = false;
            leagueManager.gameObject.SetActive(false);
        }

        public void OnPressedClubButton() {
            state = State.ClubMenu;
            StartCoroutine(AnimationTransit.Transition(menuTransit, clubTransit));
            //clubManager.enabled = true;
            clubManager.gameObject.SetActive(true);
            clubManager.OnStart();
        }

        public void OnCancelClub() {
            state = State.Menu;
            StartCoroutine(AnimationTransit.Transition(clubTransit, menuTransit));
            clubManager.OnCancel();
            //clubManager.enabled = false;
            clubManager.gameObject.SetActive(false);
        }

        public void OnCancelCreateRoom() {
            state = State.ClubMenu;
            clubManager.OnCancelCreateRoom();
        }

        public override void OnConnectedToMaster() {
            if (!connected) {
                Debug.Log("OnConnectedToMaster");
                connected = true;

                PhotonNetwork.playerName = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
#if !DEBUG
                StartCoroutine(AnimationTransit.Out(connectingTransit));
#endif
            }
        }

        public override void OnDisconnectedFromPhoton() { 
            Debug.Log($"MenuManager::OnDisconnectedFromPhoton()");
            if (connected) {
                Debug.Log("サーバーから切断されました。");
            } else {
                connectingPanel.Text.text = "インターネットに接続できません。\nタイトルに戻ります。";
                connectingPanel.indicator.SetActive(false);
            }
        }
    }
}