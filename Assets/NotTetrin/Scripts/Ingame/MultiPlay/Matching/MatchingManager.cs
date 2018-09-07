using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using NotTetrin.Constants;
using NotTetrin.SceneManagement;

namespace NotTetrin.Ingame.MultiPlay.Matching {
    public class MatchingManager : MonoBehaviour {
        [SerializeField]
        private GameObject matchingWindow;
        [SerializeField]
        private Text messageLabel;
        [SerializeField]
        private Text statusLabel;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Text startingCounter;

        private bool quit = false;
        private bool foundOpponent = false;

        private string PlayerName => IdentificationNameUtility.ParseName(PhotonNetwork.LocalPlayer.NickName);

        private void Awake() {
            var state = PhotonNetwork.NetworkClientState;
            Debug.Log("PhotonNetwork.connectionStateDetailed: " + state);

            if (state == ClientState.PeerCreated) {
                connectToPhoton();
            }
            else if (state == ClientState.ConnectedToMasterserver) {
                joinLobby();
            }
        }

        private void Start() {
            matchingWindow.SetActive(true);
            matchingWindow.GetComponent<Animator>().Play(@"OpenWindow");
            startingCounter.text = "";
        }

        private void Update() {
            if (!foundOpponent && Input.GetButtonDown("Escape")) {
                CancelMatching();
            }
        }

        private void connectToPhoton() {
            Debug.Log("Connecting to Server...");
            statusLabel.text = @"接続中";
            PhotonNetwork.ConnectUsingSettings();
        }

        private void joinLobby() {
            PhotonNetwork.JoinLobby();
        }

        private IEnumerator requestJoinRandomRoom() {
            yield return new WaitUntil(() => PhotonNetwork.InLobby);
            PhotonNetwork.JoinRandomRoom();
        }

        public void StartMatching() {
            Debug.Log(@"StartMatching");
            statusLabel.text = $"あなた: {PlayerName}";
            StartCoroutine(requestJoinRandomRoom());
        }

        public void CancelMatching() {
            quit = true;
            StopAllCoroutines();
            matchingWindow.GetComponent<Animator>().Play(@"CloseWindow");

            PhotonNetwork.Disconnect();
            SceneController.Instance.LoadScene(SceneName.Title, 0.7f);
        }

        private IEnumerator observeRoomStatus() {
            var timeoutSeconds = Random.Range(5.0f, 10.0f);
            yield return new WaitForSeconds(timeoutSeconds);
            if (foundOpponent) { yield break; }

            PhotonNetwork.LeaveRoom();

            var retrySeconds = Random.Range(0.0f, 10.0f);
            yield return new WaitForSeconds(retrySeconds);

            StartMatching();
        }

        private IEnumerator successMatching() {
            Debug.Log(@"Found the other player. Will begin transit to network battle scene, Wait a moment...");

            foundOpponent = true;

            messageLabel.text = string.Format("対戦相手が見つかりました！\n<size=25>あなたは<size=40>{0}</size>です</size>", PhotonNetwork.LocalPlayer.ActorNumber == 1 ? "<color=red>1P</color>" : "<color=blue>2P</color>");
            statusLabel.text = @"あいて: " + IdentificationNameUtility.ParseName(PhotonNetwork.PlayerListOthers[0].NickName);
            cancelButton.interactable = false;

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.IsMessageQueueRunning = false;

            updateStartingCounter(3);
            yield return new WaitForSeconds(1.0f);
            updateStartingCounter(2);
            yield return new WaitForSeconds(1.0f);
            updateStartingCounter(1);
            yield return new WaitForSeconds(1.0f);
            updateStartingCounter(0);

            SceneController.Instance.LoadScene(SceneName.NetworkBattle, 0.4f);
        }

        private void updateStartingCounter(int count) {
            startingCounter.text = $"<size=36>{count}</size><size=18> </size>秒後に開始します...";
        }

        public void OnPhotonPlayerConnected(Player player) {
            Debug.Log($"Joined player(id: { player.ActorNumber }) in this room.");

            if (PhotonNetwork.PlayerListOthers.Length > 0) {
                StartCoroutine(successMatching());
            }
        }

        public void OnPhotonPlayerDisconnected(Player player) {
            Debug.Log($"A player(id: { player.ActorNumber }) has left room.");
        }

        public void OnJoinedRoom() {
            Debug.Log("OnJoinedRoom");

            if (PhotonNetwork.PlayerListOthers.Length > 0) {
                StartCoroutine(successMatching());
            } else {
                StartCoroutine(observeRoomStatus());
            }
        }

        public void OnJoinedLobby() {
            Debug.Log("OnJoinedLobby");

            var name = PlayerPrefs.GetString(PlayerPrefsKey.PlayerName);
            var id = PhotonNetwork.AuthValues.UserId;
            PhotonNetwork.LocalPlayer.NickName = IdentificationNameUtility.Create(name, id);

            StartMatching();
        }

        public void OnPhotonCreateRoomFailed() {
            Debug.Log("OnPhotonCreateRoomFailed");
            statusLabel.text = @"ルーム作成に失敗";
        }

        public void OnPhotonRandomJoinFailed() {
            Debug.Log("OnPhotonRandomJoinFailed");
            PhotonNetwork.CreateRoom("", new RoomOptions { MaxPlayers = 2 }, null);
        }

        public void OnCreatedRoom() {
            Debug.Log("OnCreatedRoom");
        }

        public void OnDisconnectedFromPhoton() {
            Debug.Log("Disconnected from Photon.");
            if (quit) { return; }
            StartCoroutine(retryConnectToPhoton());
        }

        private IEnumerator retryConnectToPhoton() {
            statusLabel.text = $"サーバーから切断されました\n3秒後にリトライします";
            yield return new WaitForSeconds(3.0f);
            connectToPhoton();
        }

        public void OnConnectedToMaster() {
            Debug.Log("OnConnectedToMaster");
            joinLobby();
        }
    }
}