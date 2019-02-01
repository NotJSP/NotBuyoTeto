using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.UI;
using NotBuyoTeto.Constants;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Ingame.Tetrin;
using NotBuyoTeto.Ingame.Buyobuyo;

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
        private Director director;
        [SerializeField]
        private WinsManager winsManager;
        [SerializeField]
        private MessageWindow messageWindow;
        [SerializeField]
        protected PerspectiveManager perspectives;

        private PhotonView photonView;
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
            var playerSideGameMode = (GameMode)PhotonNetwork.player.CustomProperties["gamemode"];
            perspectives.Activate(PlayerSide.Player, playerSideGameMode);
            var opponentSideGameMode = (GameMode)PhotonNetwork.otherPlayers[0].CustomProperties["gamemode"];
            perspectives.Activate(PlayerSide.Opponent, opponentSideGameMode);

            director.SetMode(playerSideGameMode, opponentSideGameMode);
            director.Initialize();

//            ui.PlayerNameLabel.text = IdentificationNameUtility.ParseName(PhotonNetwork.player.NickName);
//            ui.OpponentNameLabel.text = IdentificationNameUtility.ParseName(PhotonNetwork.otherPlayers[0].NickName);

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
        private void backToMatching() => exit(SceneName.Matching);

        private void exit(string scene) {
            quit = true;
            StopAllCoroutines();

            // タイトルに戻る場合はネットワークを切断
            if (scene == SceneName.Title) {
                if (PhotonNetwork.connected) { PhotonNetwork.Disconnect(); }
            }
            // マッチングに戻る場合はルームから退室
            if (scene == SceneName.Matching) {
                if (PhotonNetwork.connected) { PhotonNetwork.LeaveRoom(); }
            }

            SceneController.Instance.LoadScene(scene, 0.7f);
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
            director.ClearObjects();
            sfxManager.Stop(IngameSfxType.GameOver);
        }

        private void ready() {
            Debug.Log(@"GameManager::ready()");
            PhotonNetwork.isMessageQueueRunning = true;
            isReady = true;

            photonView.RPC(@"OnReadyOpponent", PhotonTargets.Others);

            if (isReadyOpponent) {
                gamestart();
            }
        }

        private void gamestart() {
            Debug.Log(@"GameManager.gamestart()");
            clearObjects();
            photonView.RPC(@"OnGamestartOpponent", PhotonTargets.Others);
            sfxManager.Play(IngameSfxType.RoundStart);
            bgmManager.RandomPlay();
            director.Next();
        }

        private void gameover() {
            gameOverTime = PhotonNetwork.time;
            photonView.RPC("OnGameoverOpponent", PhotonTargets.Others, gameOverTime);
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
                gamestart();
            }
        }

        [PunRPC]
        private void OnGamestartOpponent() {
            Debug.Log("OnGamestartOpponent");
        }

        [PunRPC]
        private void OnGameoverOpponent(double timestamp) {
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

            bgmManager.Stop();
            director.GameOver();
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
