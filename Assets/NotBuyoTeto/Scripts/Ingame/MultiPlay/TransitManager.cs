using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;
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
            if (PhotonNetwork.InRoom) {
                Debug.Log("PhotonNetwork.InRoom");
                backButton.Inactive();
                waitingManager.gameObject.SetActive(true);
                waitingManager.InMenu(() => backButton.Active());
                return;
            }

            if (PhotonNetwork.InLobby) {
                Debug.Log("PhotonNetwork.InLobby");
                if (PhotonNetwork.CurrentLobby.Name == LobbyManager.LeagueLobby.Name) {
                    Debug.Log("LeagueLobby");
                    backButton.Inactive();
                    leagueManager.gameObject.SetActive(true);
                    leagueManager.InMenu(() => backButton.Active());
                    return;
                }

                if (PhotonNetwork.CurrentLobby.Name == LobbyManager.ClubLobby.Name) {
                    Debug.Log("ClubLobby");
                    backButton.Inactive();
                    clubManager.gameObject.SetActive(true);
                    clubManager.InMenu(() => backButton.Active());
                    return;
                }
            }

            backButton.Inactive();
            menuManager.gameObject.SetActive(true);
            menuManager.InMenu(() => backButton.Active());
        }
    }
}
