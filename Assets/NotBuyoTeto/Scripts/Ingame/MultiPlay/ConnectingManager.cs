using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using NCMB;
using NotBuyoTeto.Utility;
using NotBuyoTeto.Constants;
using NotBuyoTeto.SceneManagement;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class ConnectingManager : MonoBehaviourPunCallbacks {
        [SerializeField]
        private ConnectingPanel connectingPanel;

        private bool connected = false;

        // private string versionsString => $"{Versions.Version}b{Versions.BuildNumber}";

        public void Start() {
            if (PhotonNetwork.IsConnected) { return; }
            connectingPanel.Show();
            PhotonNetwork.ConnectUsingSettings();
        }

        private void backToTitle() {
            SceneController.Instance.LoadScene(SceneName.Title, SceneTransition.Duration);
        }

        private void fetchPlayerStats(Action<PlayerStats> callback) {
            var userId = PlayerPrefs.GetString(PlayerPrefsKey.PlayerId);
            var query = new NCMBQuery<NCMBObject>(NCMBClassName.PlayerStats);
            query.WhereEqualTo("userId", userId);
            query.FindAsync((objList, e) => {
                if (e != null) {
                    Debug.LogError(e.Message);
                    return;
                }
                NCMBObject obj = null;
                if (objList.Count > 0) {
                    obj = objList[0];
                } else { 
                    obj = initializeStats(userId);
                }
                var rating = Convert.ToInt32(obj["rating"]);
                var battleCount = Convert.ToInt32(obj["battleCount"]);
                var winCount = Convert.ToInt32(obj["winCount"]);
                var stats = new PlayerStats(rating, battleCount, winCount);
                callback.Invoke(stats);
            });
        }

        private NCMBObject initializeStats(string userId) {
            var obj = new NCMBObject(NCMBClassName.PlayerStats);
            obj["userId"] = userId;
            obj["rating"] = 1500;
            obj["battleCount"] = 0;
            obj["winCount"] = 0;
            obj.SaveAsync(e => {
                if (e != null) {
                    Debug.LogError(e.Message);
                }
            });
            return obj;
        }

        private void onPlayerStatsFetched(PlayerStats stats) {
            Debug.Log("ConnectingManager::onPlayerStatsFetched");
            var hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable["rating"] = stats.Rating;
            hashtable["battleCount"] = stats.BattleCount;
            hashtable["winCount"] = stats.WinCount;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);

            connectingPanel.Hide();
            connected = true;
        }

        public override void OnConnectedToMaster() {
            Debug.Log("ConnectingManager::OnConnectedToMaster()");
            if (!connected) {
                PhotonNetwork.NickName = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
                fetchPlayerStats(this.onPlayerStatsFetched);
            }
        }

        public override void OnDisconnected(DisconnectCause cause) {
            if (cause != DisconnectCause.DisconnectByClientLogic) {
                if (!connectingPanel.IsShow) { connectingPanel.Show(); }
                connectingPanel.indicator.SetActive(false);

                if (connected) {
                    connectingPanel.Text.text = "通信が切断されました。\nタイトルに戻ります。";
                } else {
                    connectingPanel.Text.text = "インターネットに接続できません。\nタイトルに戻ります。";
                }
            }

            Invoke("backToTitle", 3.0f);
        }
    }
}
