using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;

namespace NotBuyoTeto.Ingame.MultiPlay.League {
    public class LeagueManager : PunBehaviour {
        public static readonly TypedLobby Lobby = new TypedLobby("LeagueLobby", LobbyType.Default);

        [SerializeField]
        private Text messageLabel;
        [SerializeField]
        private Text statusLabel;

        private string playerName => PhotonNetwork.playerName;

        private bool joinedLobby = false;

        public void OnStart() {
            Debug.Log(@"LeagueManager::OnStart");
            PhotonNetwork.JoinLobby(Lobby);
        }

        public void OnCancel() {
            Debug.Log(@"LeagueManager::OnCancel");
            if (PhotonNetwork.inRoom) {
                PhotonNetwork.LeaveRoom();
            }
            if (PhotonNetwork.connectionStateDetailed == ClientState.Authenticating || PhotonNetwork.connectionStateDetailed == ClientState.ConnectingToGameserver) { 
                PhotonNetwork.LeaveLobby();
            }
            joinedLobby = false;
        }

        public override void OnJoinedLobby() {
            // Photonのバグ?でOnJoinedLobbyのコールバックが多重登録されるので対策
            if (!joinedLobby) {
                Debug.Log("LeagueManager::OnJoinedLobby");
                statusLabel.text = $"あなた: {playerName}";
                PhotonNetwork.JoinRandomRoom();
                joinedLobby = true;
            }
        }

        public override void OnLeftRoom() {
            Debug.Log("LeagueManager::OnLeftRoom");
        }

        public override void OnLeftLobby() {
            Debug.Log("LeagueManager::OnLeftLobby");
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
            Debug.Log("LeagueManager::OnPhotonRandomJoinFailed");
            var options = new RoomOptions { MaxPlayers = 2 };
            PhotonNetwork.CreateRoom("", options, Lobby);
        }

        public override void OnCreatedRoom() {
            Debug.Log("LeagueManager::OnCreatedRoom");
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
            Debug.Log("LeagueManager::OnPhotonCreateRoomFailed");
            statusLabel.text = @"ルーム作成に失敗しました";
        }
    }
}