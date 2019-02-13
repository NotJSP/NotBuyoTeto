using System;
using System.Collections;
using UnityEngine;
using NotBuyoTeto.Ingame.MultiPlay.Menu;
using NotBuyoTeto.Ingame.MultiPlay.League;
using NotBuyoTeto.Ingame.MultiPlay.Club;
using NotBuyoTeto.Ingame.MultiPlay.Waiting;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class TransitManager : MonoBehaviour {
        [SerializeField]
        private MenuManager menuManager;
        [SerializeField]
        private LeagueManager leagueManager;
        [SerializeField]
        private ClubManager clubManager;
        [SerializeField]
        private WaitingManager waitingManager;

        [SerializeField]
        private BackButton backButton;

        private void Awake() {
            menuManager.gameObject.SetActive(false);
            leagueManager.gameObject.SetActive(false);
            clubManager.gameObject.SetActive(false);
            waitingManager.gameObject.SetActive(false);

            if (PhotonNetwork.inRoom) {
                Debug.Log("PhotonNetwork.inRoom");
                backButton.Inactive();
                waitingManager.gameObject.SetActive(true);
                waitingManager.InMenu();
                return;
            }

            if (PhotonNetwork.lobby.Equals(LobbyManager.LeagueLobby)) {
                backButton.Inactive();
                leagueManager.gameObject.SetActive(true);
                leagueManager.InMenu(() => backButton.Active());
                return;
            }

            if (PhotonNetwork.lobby.Equals(LobbyManager.ClubLobby)) {
                backButton.Inactive();
                clubManager.gameObject.SetActive(true);
                clubManager.InMenu(() => backButton.Active());
                return;
            }

            backButton.Inactive();
            menuManager.gameObject.SetActive(true);
            menuManager.InMenu(() => backButton.Active());
        }
    }
}
