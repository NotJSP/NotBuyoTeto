﻿using System;
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
        private Director director;
        [SerializeField]
        private UIManager ui;
        [SerializeField]
        private PerspectiveManager perspectives;
        [SerializeField]
        private MinoManager minoManager;
        [SerializeField]
        private BuyoManager buyoManager;
        [SerializeField]
        private GarbageMinoManager garbageMinoManager;
        [SerializeField]
        private WinsManager winsManager;
        [SerializeField]
        private MessageWindow messageWindow;

        private PhotonView photonView;
        private double gameOverTime = 0.0;
        private bool isReady = false;
        private bool isReadyOpponent = false;
        private bool acceptedResult = false;
        private bool quit = false;

        private static readonly float BackToMatchingSeconds = 3.0f;

        protected override void Awake() {
            base.Awake();
            photonView = GetComponent<PhotonView>();
            PhotonNetwork.sendRate = 15;
            PhotonNetwork.sendRateOnSerialize = 15;
            Debug.Log("PhotonNetwork.isMessageQueueRunning: " + PhotonNetwork.isMessageQueueRunning);
            PhotonNetwork.isMessageQueueRunning = false;
        }

        protected override void OnSceneReady(object sender, EventArgs args) {
            minoManager.HitMino += onHitMino;
            groupManager.MinoDeleted += onMinoDeleted;

            ui.PlayerNameLabel.text = IdentificationNameUtility.ParseName(PhotonNetwork.player.NickName);
            ui.OpponentNameLabel.text = IdentificationNameUtility.ParseName(PhotonNetwork.otherPlayers[0].NickName);
            ui.PlayerYouLabel.enabled = true;

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
            Debug.Log(@"GameManager.resetReadyFlags()");
            isReady = false;
            isReadyOpponent = false;
        }

        private void resetObjects() {
            Debug.Log(@"GameManager.resetObjects()");
            acceptedResult = false;
            minoManager.Reset();
            garbageMinoManager.Clear();
            director.RoundStart();
            sfxManager.Stop(IngameSfxType.GameOver);
        }

        private void ready() {
            Debug.Log(@"GameManager.ready()");
            PhotonNetwork.isMessageQueueRunning = true;
            isReady = true;

            photonView.RPC(@"OnReadyOpponent", PhotonTargets.Others);

            if (isReadyOpponent) {
                gamestart();
            }
        }

        private void gamestart() {
            Debug.Log(@"GameManager.gamestart()");
            resetObjects();
            photonView.RPC(@"OnGamestartOpponent", PhotonTargets.Others);
            sfxManager.Play(IngameSfxType.RoundStart);
            bgmManager.RandomPlay();
            minoManager.Next();
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
            minoManager.Next();
        }

        private void onMinoDeleted(object sender, DeleteMinoInfo info) {
            photonView.RPC("OnDeleteMinoOpponent", PhotonTargets.Others, info.LineCount, info.ObjectCount);
        }

        private void onHitMino(object sender, EventArgs args) {
            minoManager.Release();

            // ゲームオーバー
            if (director.IsGameOver) {
                gameover();
            } else {
                groupManager.DeleteMino();
                StartCoroutine(fallGarbageAndNext());
            }
        }

        private IEnumerator fallGarbageAndNext() {
            if (garbageMinoManager.Fall()) {
                yield return new WaitWhile(() => garbageMinoManager.IsFalling);
            }
            next();
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
            minoManager.Destroy();

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
            director.Floor.SetActive(false);
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
        private void OnDeleteMinoOpponent(int lineCount, int objectCount) {
            var info = new DeleteMinoInfo(lineCount, objectCount);
            garbageMinoManager.Add(info);
        }

        [PunRPC]
        private void OnUpdateOpponentPing(int ping) {
            ui.OpponentPingLabel.text = $"Ping: { ping }ms";
        }
    }
}
