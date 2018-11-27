using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;

namespace NotBuyoTeto.Ingame.MultiPlay.League {
    public class MatchingManager : MonoBehaviourPunCallbacks {
        [SerializeField]
        private GameObject matchingWindow;
        [SerializeField]
        private Text messageLabel;
        [SerializeField]
        private Text statusLabel;
        [SerializeField]
        private Button cancelButton;

        private string playerName => PhotonNetwork.LocalPlayer.NickName;

        public void StartMatching() {
            Debug.Log(@"StartMatching");
            PhotonNetwork.JoinLobby();
        }

        public void CancelMatching() {
            Debug.Log(@"CancelMatching");
            Debug.Log(PhotonNetwork.NetworkClientState);
            if (PhotonNetwork.InRoom) {
                Debug.Log("InRoom");
                PhotonNetwork.LeaveRoom();
            }
            if (PhotonNetwork.NetworkClientState == ClientState.Authenticating || PhotonNetwork.NetworkClientState == ClientState.ConnectingToGameserver) { 
                PhotonNetwork.LeaveLobby();
            }
        }

        public override void OnJoinedLobby() {
            Debug.Log("OnJoinedLobby");
            PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            statusLabel.text = $"あなた: {playerName}";
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnLeftRoom() {
            Debug.Log("OnLeftRoom");
        }

        public override void OnLeftLobby() {
            Debug.Log("OnLeftLobby");
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            Debug.Log("OnJoinRandomFailed");
            PhotonNetwork.CreateRoom("", new RoomOptions { MaxPlayers = 2 }, null);
        }

        public override void OnCreatedRoom() {
            Debug.Log("OnCreatedRoom");
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            Debug.Log("OnCreateRoomFailed");
            statusLabel.text = @"ルーム作成に失敗しました";
        }
    }
}