using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class UIManager : MonoBehaviour {
        [Header("Player")]
        public Text PlayerPingLabel;
        public Text PlayerNameLabel;
        public WinsCounter PlayerWinsCounter;
        [Header("Opponent")]
        public Text OpponentPingLabel;
        public Text OpponentNameLabel;
        public WinsCounter OpponentWinsCounter;
    }
}
