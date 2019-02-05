using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using NotBuyoTeto.Utility;
using NotBuyoTeto.Constants;
using NotBuyoTeto.SceneManagement;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class ConnectingManager : PunBehaviour {
        [SerializeField]
        private ConnectingPanel connectingPanel;

        private bool connected = false;

        private string versionsString => $"{Versions.Version}b{Versions.BuildNumber}";

        private void Start() {
            if (PhotonNetwork.connected) { return; }
            connectingPanel.Show();
            PhotonNetwork.ConnectUsingSettings(versionsString);
        }

        private void backToTitle() {
            SceneController.Instance.LoadScene(SceneName.Title, SceneTransition.Duration);
        }

        public override void OnConnectedToMaster() {
            if (!connected) {
                connected = true;
                PhotonNetwork.playerName = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
                connectingPanel.Hide();
            }
        }

        public override void OnDisconnectedFromPhoton() {
            if (!connectingPanel.IsShow) { connectingPanel.Show(); }
            connectingPanel.indicator.SetActive(false);

            if (connected) {
                connectingPanel.Text.text = "通信が切断されました。\nタイトルに戻ります。";
            } else {
                connectingPanel.Text.text = "インターネットに接続できません。\nタイトルに戻ります。";
            }

            Invoke("backToTitle", 3.0f);
        }
    }
}
