using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using NotTetrin.Ingame.MultiPlay.Marathon;
using NotTetrin.Ingame.Marathon;

namespace NotTetrin.Ingame.MultiPlay {
    public class NetworkDirector : Director {
        [SerializeField] GameObject[] floors;
        [SerializeField] Ceiling[] ceilings;
        [SerializeField] HoldMino[] holdMinos;
        [SerializeField] NextMino[] nextMinos;

        [SerializeField] Text[] pingLabels;
        [SerializeField] Text[] youLabels;
        [SerializeField] Text[] nameLabels;
        [SerializeField] WinsCounter[] winsCounters;

        public int PlayerIndex => (PhotonNetwork.LocalPlayer.ActorNumber - 1);
        public int OpponentIndex => (1 - PlayerIndex);

        public override GameObject Floor => floors[PlayerIndex];
        public override Ceiling Ceiling => ceilings[PlayerIndex];
        public override HoldMino HoldMino => holdMinos[PlayerIndex];
        public override NextMino NextMino => nextMinos[PlayerIndex];

        public Text PlayerPingLabel => pingLabels[PlayerIndex];
        public Text PlayerYouLabel => youLabels[PlayerIndex];
        public Text PlayerNameLabel => nameLabels[PlayerIndex];
        public WinsCounter PlayerWinsCounter => winsCounters[PlayerIndex];
        public Text OpponentPingLabel => pingLabels[OpponentIndex];
        public Text OpponentYouLabel => youLabels[PlayerIndex];
        public Text OpponentNameLabel => nameLabels[OpponentIndex];
        public WinsCounter OpponentWinsCounter => winsCounters[OpponentIndex];
    }
}
