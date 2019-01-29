using System;
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
        private ASyncValue<List<Ranker>, NCMBException> topRankers = new ASyncValue<List<Ranker>, NCMBException>();

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
            builder = new StringBuilder();

            while (true) { 
                currentRank.Reset();
                fetchRank(type, score);
                yield return new WaitUntil(currentRank.TakeOrFailure);

                if (!currentRank.Failure) { break; }
                yield return new WaitForSeconds(3.0f);
            }

            while (true) { 
                topRankers.Reset();
                fetchRankers(type);
                yield return new WaitUntil(topRankers.TakeOrFailure);

                if (!topRankers.Failure) { break; }
                yield return new WaitForSeconds(3.0f);
            }
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

        private void fetchRankers(RankingType type) {
            var className = getClassName(type);
            var query = new NCMBQuery<NCMBObject>(className);
            query.OrderByDescending(@"score");
            query.Limit = FetchCount;
            query.FindAsync((list, e) => {
                if (e != null) {
                    Debug.LogError(e);
                    topRankers.Exception = e;
                    return;
                }

                var rankers = new List<Ranker>();
                foreach (var obj in list) {
                    var name = Convert.ToString(obj["name"]);
                    var score = Convert.ToInt32(obj["score"]);
                    rankers.Add(new Ranker(name, score));
                }
                topRankers.Value = rankers;

                for (int i = 0; i < rankers.Count; i++) {
                    var str = rankers[i].ToString();

                    if (i < rankers.Count - 1) {
                        builder.AppendLine(str);
                    } else {
                        builder.Append(str);
                    }
                }

                textField.text = builder.ToString();
            });
        }

        public bool Save(RankingType type, Ranker ranker) {
            if (string.IsNullOrWhiteSpace(ranker.Name)) {
                throw new ArgumentException(@"空の名前でランキングに登録する事はできません。");
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
            var className = getClassName(type);
            var ncmbObj = new NCMBObject(className);
            ncmbObj[@"name"] = ranker.Name;
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
                    PlayerPrefs.SetString(key, ncmbObj.ObjectId);
                });
                yield return new WaitUntil(watcher.TakeOrFailure);

                if (!watcher.Failure) { break; }
                yield return new WaitForSeconds(3.0f);
            }

            Fetch(type, ranker.Score);
        }
    }
}