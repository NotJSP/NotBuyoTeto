using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;
using NotBuyoTeto.Ingame.MultiPlay.League;
using NotBuyoTeto.Ingame.MultiPlay.Club;

namespace NotBuyoTeto.Ingame.MultiPlay.Menu {
    public class MenuManager : PunBehaviour {
        [SerializeField]
        private BackButton backButton;

        [SerializeField]
        private LeagueManager leagueManager;
        [SerializeField]
        private ClubManager clubManager;

        [SerializeField]
        private GameObject mainPanel;
        [SerializeField]
        private ConnectingPanel connectingPanel;

        private AnimationTransitEntry menuTransit;
        private AnimationTransitEntry connectingTransit;

        private bool connected = false;

        private void Awake() {
            this.menuTransit = new AnimationTransitEntry(mainPanel, "Menu In", "Menu Out");
            this.connectingTransit = new AnimationTransitEntry(connectingPanel.gameObject, "Panel In", "Panel Out");
        }

        private void Start() {
#if !DEBUG
            StartCoroutine(AnimationTransit.In(connectingTransit));
#endif
            PhotonNetwork.ConnectUsingSettings("1.0");
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
            StartCoroutine(AnimationTransit.In(menuTransit, afterAction));
        }

        public void OutMenu(Action afterAction = null) {
            StartCoroutine(AnimationTransit.Out(menuTransit, afterAction));
        }

        public void back(object sender, EventArgs args) {
            if (PhotonNetwork.connected) {
                PhotonNetwork.Disconnect();
            }
            SceneController.Instance.LoadScene(SceneName.Title, SceneTransition.Duration);
        }

        public void OnPressedLeagueButton() {
            backButton.Inactive();
            OutMenu(() => {
                leagueManager.gameObject.SetActive(true);
                leagueManager.InMenu(() => backButton.Active());
                gameObject.SetActive(false);
                leagueManager.OnStart();
            });
        }

        public void OnPressedClubButton() {
            backButton.Inactive();
            OutMenu(() => {
                clubManager.gameObject.SetActive(true);
                clubManager.InMenu(() => backButton.Active());
                gameObject.SetActive(false);
                clubManager.OnStart();
            });
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