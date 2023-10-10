using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;
using NotBuyoTeto.Constants;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame.SinglePlay {
    public class RankingManager : MonoBehaviour {
        private Coroutine fetchCoroutine = null;
        private Coroutine saveCoroutine = null;

        private static readonly int FetchCount = 10;

        [SerializeField]
        private Text textField;

        private ASyncValue<int, NCMBException> currentRank = new ASyncValue<int, NCMBException>();

        private StringBuilder builder;

        private string getClassName(RankingType type) {
            switch (type) {
                case RankingType.MarathonMode:
                    return @"MarathonRanking";
                case RankingType.TokotonMode:
                    return @"TokotonRanking";
                default:
                    throw new ArgumentOutOfRangeException("Invalid type was specified.");
            }
        }

        public void Fetch(RankingType type, int score) {
            if (fetchCoroutine != null) { return; }

            try {
                fetchCoroutine = StartCoroutine(fetchAll(type, score));
            } catch (Exception e) {
                Debug.LogError(e);
                textField.text = @"ランキングの取得に失敗";
            } finally {
                fetchCoroutine = null;
            }
        }

        private IEnumerator fetchAll(RankingType type, int score) {
            this.builder = new StringBuilder();

            currentRank.Reset();
            while (true) { 
                fetchRank(type, score);
                yield return new WaitUntil(currentRank.TakeOrFailure);
                if (currentRank.TakeAndSucceeded) { break; }
                yield return new WaitForSeconds(3.0f);
            }

            var rankers = new ASyncValue<List<Ranker>, NCMBException>();
            while (true) {
                fetchRankers(type, rankers);
                yield return new WaitUntil(rankers.TakeOrFailure);
                if (rankers.TakeAndSucceeded) { break; }
                yield return new WaitForSeconds(3.0f);
            }

            yield return fetchUsernames(rankers.Value);
        }

        private void fetchRank(RankingType type, int score) {
            var className = getClassName(type);
            var query = new NCMBQuery<NCMBObject>(className);
            query.WhereGreaterThan(@"score", score);
            query.CountAsync((count, e) => {
                if (e != null) {
                    Debug.LogError(e);
                    currentRank.Exception = e;
                    return;
                }

                currentRank.Value = count + 1;

                builder.AppendLine($"あなたの順位: { currentRank.Value } 位");
                builder.AppendLine(@"============================");
                textField.text = builder.ToString();
            });
        }

        private void fetchRankers(RankingType type, ASyncValue<List<Ranker>, NCMBException> results) {
            var className = getClassName(type);
            var query = new NCMBQuery<NCMBObject>(className);
            query.OrderByDescending(@"score");
            query.Limit = FetchCount;
            query.FindAsync((list, e) => {
                if (e != null) {
                    Debug.LogError(e);
                    results.Exception = e;
                    return;
                }

                var rankers = new List<Ranker>();
                foreach (var obj in list) {
                    var userId = Convert.ToString(obj["userId"]);
                    var score = Convert.ToInt32(obj["score"]);
                    rankers.Add(new Ranker(userId, score));
                }
                results.Value = rankers;
            });
        }

        private IEnumerator fetchUsernames(List<Ranker> rankers) {
            var userIds = rankers.Select(r => r.UserId).ToArray();
            var result = new ASyncValue<Dictionary<string, string>, NCMBException>();
            UserManager.FetchUsernames(userIds, result);
            yield return new WaitUntil(result.TakeOrFailure);
            var nameTable = result.Value;

            for (var i = 0; i < rankers.Count; i++) {
                var name = nameTable[rankers[i].UserId];
                var score = rankers[i].Score;
                var str = $"{name}: {score}";

                if (i < rankers.Count - 1) {
                    this.builder.AppendLine(str);
                } else {
                    this.builder.Append(str);
                }
            }

            textField.text = this.builder.ToString();
        }

        public bool Save(RankingType type, Ranker ranker) {
            if (string.IsNullOrWhiteSpace(ranker.UserId)) {
                throw new ArgumentException(@"空のIDでランキングに登録する事はできません。");
            }

            if (saveCoroutine != null) {
                StopCoroutine(saveCoroutine);
            }

            try {
                saveCoroutine = StartCoroutine(saveRank(type, ranker));
            } catch (Exception e) {
                Debug.LogError(e.Message);
                return false;
            } finally {
                saveCoroutine = null;
            }

            return true;
        }

        private IEnumerator saveRank(RankingType type, Ranker ranker) {
            var className = this.getClassName(type);

            var ncmbObj = new NCMBObject(className);
            ncmbObj[@"userId"] = ranker.UserId;
            ncmbObj[@"score"] = ranker.Score;

            var key = PlayerPrefsKey.ObjectId[type];
            if (PlayerPrefs.HasKey(key)) {
                ncmbObj.ObjectId = PlayerPrefs.GetString(key);
            }

            var watcher = new ASyncValue<bool, NCMBException>();

            while (true) {
                ncmbObj.SaveAsync(e => {
                    if (e != null) {
                        Debug.LogError(e);
                        watcher.Exception = e;
                        return;
                    }
                    watcher.Value = true;
                });
                yield return new WaitUntil(watcher.TakeOrFailure);

                if (watcher.TakeAndSucceeded) {
                    PlayerPrefs.SetString(key, ncmbObj.ObjectId);
                    break;
                }
                yield return new WaitForSeconds(3.0f);
            }

            Fetch(type, ranker.Score);
        }
    }
}