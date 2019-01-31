using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NotBuyoTeto.Ingame.MultiPlay.Waiting {
    public class WaitingPanel : MonoBehaviour {
        [SerializeField]
        private Text nameField;
        [SerializeField]
        private Text recordField;
        [SerializeField]
        private Text ratingField;

        public static string FormatRecord(FightRecord record) {
            return $"{record.FightCount:0000}戦 {record.WinCount:0000}勝 {record.LoseCount:0000}敗 (勝率: {record.WinRate * 100:00.0}%)";
        }

        public static string FormatRating(int rating) {
            return $"レート: <size=32><b>{rating}</b></size>";
        }

        public void Set(WaitingPlayer player) {
            nameField.text = player.Name;
            recordField.text = FormatRecord(player.FightRecord);
            ratingField.text = FormatRating(player.Rating);
        }
    }
}