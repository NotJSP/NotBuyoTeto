using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NotBuyoTeto.Ingame.MultiPlay.Battle {
    public class UIManager : MonoBehaviour {
        [Header("Player")]
        public Text PlayerPingLabel;
        public Text PlayerYouLabel;
        public Text PlayerNameLabel;
        public WinsCounter PlayerWinsCounter;
        [Header("Opponent")]
        public Text OpponentPingLabel;
        public Text OpponentYouLabel;
        public Text OpponentNameLabel;
        public WinsCounter OpponentWinsCounter;
    }
}
