using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotBuyoTeto.Ingame.MultiPlay.Menu;
using NotBuyoTeto.Ingame.MultiPlay.League;
using NotBuyoTeto.Ingame.MultiPlay.Club;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class TransitManager : MonoBehaviour {
        enum State {
            Menu,
            League,
            Club,
        }

        [SerializeField]
        private MenuManager menuManager;
        [SerializeField]
        private LeagueManager leagueManager;
        [SerializeField]
        private ClubManager clubManager;

    }
}
