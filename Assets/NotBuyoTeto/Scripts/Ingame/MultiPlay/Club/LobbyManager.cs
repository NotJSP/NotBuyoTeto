using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NotBuyoTeto.SceneManagement;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    public class LobbyManager : MonoBehaviourPunCallbacks {
        [SerializeField]
        private GameObject clubPanel;

        private AnimationTransitEntry clubTransit;
        private AnimationTransitEntry createRoomTransit;

        private void Awake() {
            this.clubTransit = new AnimationTransitEntry(clubPanel, "In Menu", "Out Menu");
            this.createRoomTransit = new AnimationTransitEntry(clubPanel, "In CreateRoom", "Out CreateRoom");
        }

        public void CreateRoomOnLobby() {
            StartCoroutine(AnimationTransit.Transition(clubTransit, createRoomTransit));
        }

        public void CreateRoomOnPanel() {
            StartCoroutine(AnimationTransit.Transition(createRoomTransit, clubTransit));
        }

        public void CancelCreateRoom() {
            StartCoroutine(AnimationTransit.Transition(createRoomTransit, clubTransit));
        }
    }
}