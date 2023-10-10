using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    public class RoomManager : MonoBehaviourPunCallbacks {
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

        public override void OnEnable() {
            base.OnEnable();
            rulePanel.Clear();
        }

        public override void OnDisable() {
            base.OnDisable();
            removeInstancedRooms();
        }

        public void CreateRoom(string name, RoomSettings settings) {
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

        public override void OnRoomListUpdate(List<RoomInfo> roominfos) {
            Debug.Log($"RoomManager::OnRoomListUpdate [room count: {roominfos.Count}]");

            if (instancedRoomEntries.Count >= 0) {
                removeInstancedRooms();
            }

            foreach (var room in roominfos) {
                if (room == null) { continue; }
                if (!room.IsOpen) { continue; }

                var properties = room.CustomProperties;

                var settings = new RoomSettings();
                if (!properties.ContainsKey("wins")) { continue; }
                settings.WinsCount = (int)properties["wins"];
                if (!properties.ContainsKey("speed")) { continue; }
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
