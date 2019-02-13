using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.UI;
using NotBuyoTeto.Constants;
using NotBuyoTeto.SceneManagement;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    [RequireComponent(typeof(PhotonView))]
    public class GameManager : SceneBase {
        [SerializeField]
        private BgmManager bgmManager;
        [SerializeField]
        private IngameSfxManager sfxManager;
        [SerializeField]
        private UIManager ui;
        [SerializeField]
        protected PerspectiveManager perspectives;
        [SerializeField]
        private DirectorManager directorManager;
        [SerializeField]
        private GarbageManager garbageManager;
        [SerializeField]
        private GarbageCalculator garbageCalculator;
        [SerializeField]
        private WinsManager winsManager;
        [SerializeField]
        private MessageWindow messageWindow;

        public GameMode PlayerMode { get; private set; }
        public GameMode OpponentMode { get; private set; }

        private PhotonView photonView;
        private Director director;
        private double gameOverTime = 0.0;
        private bool isReady = false;
        private bool isReadyOpponent = false;
        private bool acceptedResult = false;
        private bool quit = false;

        private static readonly float BackToMatchingSeconds = 3.0f;

        protected void Awake() {
            photonView = GetComponent<PhotonView>();
            PhotonNetwork.sendRate = 15;
            PhotonNetwork.sendRateOnSerialize = 15;
        }

        protected override void OnSceneReady(object sender, EventArgs args) {
            this.PlayerMode = (GameMode)PhotonNetwork.player.CustomProperties["mode"];
            this.OpponentMode = (GameMode)PhotonNetwork.otherPlayers[0].CustomProperties["mode"];

            perspectives.Activate(PlayerSide.Player, PlayerMode);
            perspectives.Activate(PlayerSide.Opponent, OpponentMode);

            directorManager.SetMode(PlayerMode);
            director = directorManager.GetDirector();
            director.OnGameOver += onGameOver;
            director.Initialize();

            ui.PlayerNameLabel.text = IdentificationNameUtility.ParseName(PhotonNetwork.player.NickName);
            ui.OpponentNameLabel.text = IdentificationNameUtility.ParseName(PhotonNetwork.otherPlayers[0].NickName);

            StartCoroutine(updateAndSendPing());

            ready();
        }

        private void Update() {
            // 強制終了 (デバッグ用)
            if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Escape)) {
                backToTitle();
            }
        }

        private void backToTitle() => exit(SceneName.Title);
        private void backToMatching() => exit(SceneName.MultiPlay);

        private void exit(string scene) {
            quit = true;
            StopAllCoroutines();

            if (PhotonNetwork.inRoom) {
//                var matchingType = (MatchingType)PhotonNetwork.room.CustomProperties["type"];
                PhotonNetwork.LeaveRoom();
            }
            
            // タイトルに戻る場合はネットワークを切断
            if (scene == SceneName.Title) {
                if (PhotonNetwork.connected) { PhotonNetwork.Disconnect(); }
            }

            SceneController.Instance.LoadScene(scene, SceneTransition.Duration);
        }

        private void OnLeftLobby() {
            Debug.Log("GameManager::OnLeftLobby");
        }

        private IEnumerator updateAndSendPing() {
            while (true) {
                var ping = PhotonNetwork.GetPing();
                ui.PlayerPingLabel.text = $"Ping: { ping }ms";
                photonView.RPC(@"OnUpdateOpponentPing", PhotonTargets.Others, ping);
                yield return new WaitForSeconds(2.0f);
            }
        }

        private void resetReadyFlags() {
            Debug.Log(@"GameManager::resetReadyFlags()");
            isReady = false;
            isReadyOpponent = false;
        }

        private void clearObjects() {
            Debug.Log(@"GameManager::clearObjects()");
            acceptedResult = false;
            director.RoundStart();
            sfxManager.Stop(IngameSfxType.GameOver);
        }

        private void ready() {
            Debug.Log(@"GameManager::ready()");
            PhotonNetwork.isMessageQueueRunning = true;
            isReady = true;

            photonView.RPC(@"OnReadyOpponent", PhotonTargets.Others);

            if (isReadyOpponent) {
                gameStart();
            }
        }

        private void gameStart() {
            Debug.Log(@"GameManager::gameStart()");
            clearObjects();
            photonView.RPC(@"OnGameStartOpponent", PhotonTargets.Others);
            sfxManager.Play(IngameSfxType.RoundStart);
            bgmManager.RandomPlay();
            garbageManager.Restart();
            garbageCalculator.Restart();
            director.GameStart();
        }

        private void onGameOver(object sender, EventArgs args) {
            gameOverTime = PhotonNetwork.time;
            photonView.RPC("OnGameOverOpponent", PhotonTargets.Others, gameOverTime);
        }

        private void win() {
            quit = true;
            messageWindow.Message = @"<size=48>あなたの<color=red>勝ち</color>!!</size>";
            messageWindow.Status = @"マッチングに戻ります";
            messageWindow.Show();
            Invoke("backToMatching", BackToMatchingSeconds);
        }

        private void lose() {
            quit = true;
            messageWindow.Message = @"あなたの<color=blue>負け</color>..";
            messageWindow.Status = @"マッチングに戻ります";
            messageWindow.Show();
            Invoke("backToMatching", BackToMatchingSeconds);
        }

        private void next() {
            director.Next();
        }

        private void OnDisconnectedFromPhoton() {
            if (quit) { return; }
            messageWindow.Message = @"通信が切断されました";
            messageWindow.Status = @"タイトルに戻ります";
            messageWindow.Show();
            Invoke("backToTitle", 3.0f);
        }

        private void OnPhotonPlayerDisconnected(PhotonPlayer player) {
            if (quit) { return; }
            messageWindow.Message = @"対戦相手が切断されました";
            messageWindow.Status = @"タイトルに戻ります";
            messageWindow.Show();
            Invoke("backToTitle", 3.0f);
        }

        [PunRPC]
        private void OnReadyOpponent() {
            Debug.Log("OnReadyOpponent");
            isReadyOpponent = true;
            if (isReady) {
                gameStart();
            }
        }

        [PunRPC]
        private void OnGameStartOpponent() {
            Debug.Log("OnGameStartOpponent");
        }

        [PunRPC]
        private void OnGameOverOpponent(double timestamp) {
            Debug.Log("OnGameoverOpponent (" + timestamp + ")");
            director.RoundEnd();

            if (timestamp < gameOverTime) {
                photonView.RPC("OnRoundWinAccepted", PhotonTargets.Others);
                OnRoundLoseAccepted();
            } else {
                photonView.RPC("OnRoundLoseAccepted", PhotonTargets.Others);
                OnRoundWinAccepted();
            }
        }

        [PunRPC]
        private void OnRoundWinAccepted() {
            if (acceptedResult) { return; }
            ui.PlayerWinsCounter.Increment();

            director.RoundEnd();
            bgmManager.Stop();
            sfxManager.Play(IngameSfxType.GameOver);
            resetReadyFlags();

            if (winsManager.Finished) {
                Invoke("win", 6.0f);
            } else {
                Invoke("ready", 9.0f);
            }
            acceptedResult = true;
        }

        [PunRPC]
        private void OnRoundLoseAccepted() {
            if (acceptedResult) { return; }
            ui.OpponentWinsCounter.Increment();

            director.GameOver();
            bgmManager.Stop();
            sfxManager.Play(IngameSfxType.GameOver);
            resetReadyFlags();

            if (winsManager.Finished) {
                Invoke("lose", 6.0f);
            } else {
                Invoke("ready", 9.0f);
            }
            acceptedResult = true;
        }

        [PunRPC]
        private void OnUpdateOpponentPing(int ping) {
            ui.OpponentPingLabel.text = $"Ping: { ping }ms";
        }
    }
}
