using System.Collections.Generic;
using UnityEngine;
using Photon;
using ExitGames.Client.Photon;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    public class RoomManager : PunBehaviour {
        [SerializeField]
        private ClubManager clubManager;
        [SerializeField]
        private Transform container;
        [SerializeField]
        private RulePanel rulePanel;
        [SerializeField]
        private GameObject fetchingPanel;

        [SerializeField]
        private RoomEntry roomPrefab;

        private List<RoomEntry> instancedRoomEntries = new List<RoomEntry>();

        private void OnEnable() {
            rulePanel.Clear();
        }

        private void OnDisable() {
            removeInstancedRooms();
        }

        public void CreateRoom(RoomSettings settings) {
            var properties = new Hashtable();
            properties["wins"] = settings.WinsCount;
            properties["speed"] = settings.FallSpeed;
            properties["type"] = MatchingType.Club;
            var options = new RoomOptions {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = 2,
                CustomRoomProperties = properties,
                CustomRoomPropertiesForLobby = new string[] { "wins", "speed", "type" },
            };

            var name = IdentificationNameUtility.Create(PhotonNetwork.playerName, PhotonNetwork.AuthValues.UserId);
            PhotonNetwork.CreateRoom(name, options, LobbyManager.ClubLobby);
        }

        public void Fetch() {
            Debug.Log("RoomManager::Fetch");
            fetchingPanel.SetActive(true);
        }

        private void removeInstancedRooms() {
            for (int i = 0; i < instancedRoomEntries.Count; i++) {
                var entry = instancedRoomEntries[i];
                Destroy(entry.gameObject);
            }
            instancedRoomEntries.Clear();
        }

        public override void OnReceivedRoomListUpdate() {
            var rooms = PhotonNetwork.GetRoomList();
            Debug.Log($"RoomManager::OnReceivedRoomListUpdate [room count: {rooms.Length}]");

            if (instancedRoomEntries.Count >= 0) {
                removeInstancedRooms();
            }

            foreach (var room in rooms) {
                if (!room.IsOpen) { continue; }

                var properties = room.CustomProperties;

                var settings = new RoomSettings();
                settings.WinsCount = (int)properties["wins"];
                settings.FallSpeed = (float)properties["speed"];

                var entry = Instantiate(roomPrefab, container);
                entry.SetPanel(clubManager, rulePanel);
                entry.RoomName = room.Name;
                entry.Settings = settings;

                instancedRoomEntries.Add(entry);
            }

            fetchingPanel.SetActive(false);
        }
    }
}
