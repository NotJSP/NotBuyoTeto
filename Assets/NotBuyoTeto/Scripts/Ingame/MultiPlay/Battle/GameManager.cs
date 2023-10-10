using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using NCMB;
using NotBuyoTeto.UI;
using NotBuyoTeto.Constants;
using NotBuyoTeto.SceneManagement;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    [RequireComponent(typeof(PhotonView))]
    public class GameManager : SceneBase {
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
        private RatingCalculator ratingCalculator;
        [SerializeField]
        private MessageWindow messageWindow;

        public GameMode PlayerMode { get; private set; }
        public GameMode OpponentMode { get; private set; }

        private PhotonView photonView;
        private Director director;
        private double gameOverTime = 0.0;  // 対戦に勝利したかどうかを判定するために使用
        private bool isReady = false;
        private bool isReadyOpponent = false;
        private bool acceptedResult = false;
        private bool quit = false;

        private Player player => PhotonNetwork.LocalPlayer;
        private Player opponent => PhotonNetwork.PlayerListOthers[0];

        private static readonly float BackToMatchingSeconds = 3.0f;

        protected void Awake() {
            photonView = GetComponent<PhotonView>();
            PhotonNetwork.SendRate = 15;
            PhotonNetwork.SerializationRate = 15;
        }

        protected override void OnSceneReady(object sender, EventArgs args) {
            this.PlayerMode = (GameMode)player.CustomProperties["mode"];
            this.OpponentMode = (GameMode)opponent.CustomProperties["mode"];

            perspectives.Activate(PlayerSide.Player, PlayerMode);
            perspectives.Activate(PlayerSide.Opponent, OpponentMode);

            directorManager.SetMode(PlayerMode);
            director = directorManager.GetDirector();
            director.OnGameOver += onGameOver;
            director.Initialize();

            ui.PlayerNameLabel.text = IdentificationNameUtility.ParseName(player.NickName);
            ui.OpponentNameLabel.text = IdentificationNameUtility.ParseName(opponent.NickName);

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

            if (PhotonNetwork.InRoom) {
                if (PhotonNetwork.CurrentLobby.Name == LobbyManager.LeagueLobby.Name) {
                    PhotonNetwork.LeaveRoom();
                }
            }
            
            // タイトルに戻る場合はネットワークを切断
            if (scene == SceneName.Title) {
                if (PhotonNetwork.IsConnected) { PhotonNetwork.Disconnect(); }
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
                photonView.RPC(@"OnUpdateOpponentPing", RpcTarget.Others, ping);
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
            PhotonNetwork.IsMessageQueueRunning = true;
            isReady = true;

            photonView.RPC(@"OnReadyOpponent", RpcTarget.Others);

            if (isReadyOpponent) {
                gameStart();
            }
        }

        private void gameStart() {
            Debug.Log(@"GameManager::gameStart()");
            clearObjects();
            photonView.RPC(@"OnGameStartOpponent", RpcTarget.Others);
            sfxManager.Play(IngameSfxType.RoundStart);
            bgmManager.RandomPlay();
            garbageManager.Restart();
            Timer.Restart();
            director.GameStart();
        }

        private void onGameOver(object sender, EventArgs args) {
            gameOverTime = PhotonNetwork.Time;
            photonView.RPC("OnGameOverOpponent", RpcTarget.Others, gameOverTime);
        }

        private void win() {
            showResult(new BattleResult(BattleResult.ResultType.Win));
        }

        private void lose() {
            showResult(new BattleResult(BattleResult.ResultType.Lose));
        }

        private void showResult(BattleResult result) {
            var stats = updateStats(result);
            var diffRating = stats.Item2.Rating - stats.Item1.Rating;
            quit = true;
            if (result.Type == BattleResult.ResultType.Win) {
                messageWindow.Message = @"<size=48>あなたの<color=red>勝ち</color>!!</size>";
                messageWindow.Status = $"レート: {stats.Item1.Rating} => <size=28><color=blue>{stats.Item2.Rating} (+{diffRating})</color></size>";
            }
            if (result.Type == BattleResult.ResultType.Lose) {
                messageWindow.Message = @"あなたの<color=blue>負け</color>..";
                messageWindow.Status = $"レート: <size=28>{stats.Item1.Rating}</size> => <color=red>{stats.Item2.Rating} ({diffRating})</color>";
            }
            messageWindow.Show();
            Invoke("backToMatching", BackToMatchingSeconds);
        }

        private Tuple<PlayerStats, PlayerStats> updateStats(BattleResult result) {
            var oldStats = getPlayerStatsFromProps(player);
            var opponentStats = getPlayerStatsFromProps(opponent);
            var newRating = ratingCalculator.NewRating(oldStats.Rating, opponentStats.Rating, result);
            if (result.Type == BattleResult.ResultType.Win) { oldStats.Win(); }
            if (result.Type == BattleResult.ResultType.Lose) { oldStats.Lose(); }
            var newStats = new PlayerStats(newRating, oldStats.BattleCount, oldStats.WinCount);
            saveStats(newStats);
            return new Tuple<PlayerStats, PlayerStats>(oldStats, newStats);
        }

        private void saveStats(PlayerStats stats) {
            var userId = PlayerPrefs.GetString(PlayerPrefsKey.PlayerId);
            var query = new NCMBQuery<NCMBObject>(NCMBClassName.PlayerStats);
            query.WhereEqualTo("userId", userId);
            query.FindAsync((objList, e) => {
                if (e != null) {
                    Debug.LogError(e.Message);
                }
                if (objList.Count == 0) {
                    Debug.LogError("Statsが取得できませんでした");
                    return;
                }
                var obj = objList[0];
                obj["rating"] = stats.Rating;
                obj["battleCount"] = stats.BattleCount;
                obj["winCount"] = stats.WinCount;
                obj.SaveAsync();

                var hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable["rating"] = stats.Rating;
                hashtable["battleCount"] = stats.BattleCount;
                hashtable["winCount"] = stats.WinCount;
                PhotonNetwork.SetPlayerCustomProperties(hashtable);
            });
        }

        private PlayerStats getPlayerStatsFromProps(Player player) {
            var props = player.CustomProperties;
            var rating = (int)props["rating"];
            var battleCount = (int)props["battleCount"];
            var winCount = (int)props["winCount"];
            return new PlayerStats(rating, battleCount, winCount);
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

        private void OnPhotonPlayerDisconnected(Player player) {
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
                photonView.RPC("OnRoundWinAccepted", RpcTarget.Others);
                OnRoundLoseAccepted();
            } else {
                photonView.RPC("OnRoundLoseAccepted", RpcTarget.Others);
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
