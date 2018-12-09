using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Photon;
using ExitGames.Client.Photon;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    public class RoomManager : PunBehaviour {
        [SerializeField]
        private Transform container;
        [SerializeField]
        private RulePanel rulePanel;
        [SerializeField]
        private GameObject fetchingPanel;

        [SerializeField]
        private RoomEntry roomPrefab;

        private List<RoomEntry> instancedRoomEntries;

        private void OnEnable() {
            rulePanel.Clear();
        }

        private void OnDisable() {
            if (instancedRoomEntries != null) {
                for (int i = 0; i < instancedRoomEntries.Count; i++) {
                    var entry = instancedRoomEntries[i];
                    Destroy(entry.gameObject);
                }
                instancedRoomEntries = null;
            }
        }

        public void CreateRoom(string name, RoomSettings settings) {
            var properties = new Hashtable();
            properties["WinsCount"] = settings.WinsCount;
            properties["FallSpeed"] = settings.FallSpeed;
            var options = new RoomOptions {
                IsOpen = true,
                IsVisible = true,
                MaxPlayers = 2,
                CustomRoomProperties = properties,
                CustomRoomPropertiesForLobby = new string[] { "WinsCount", "FallSpeed" },
            };
            PhotonNetwork.CreateRoom(name, options, ClubManager.Lobby);
        }

        public void Fetch() {
            Debug.Log("RoomManager::Fetch");
            PhotonNetwork.GetCustomRoomList(ClubManager.Lobby, null);
            fetchingPanel.SetActive(true);
        }

        public override void OnReceivedRoomListUpdate() {
            var rooms = PhotonNetwork.GetRoomList();
            Debug.Log($"RoomManager::OnReceivedRoomListUpdate [room count: {rooms.Length}]");

            instancedRoomEntries = new List<RoomEntry>(rooms.Length);

            foreach (var room in rooms) {
                var properties = room.CustomProperties;

                var settings = new RoomSettings();
                settings.WinsCount = (int)properties["WinsCount"];
                settings.FallSpeed = (float)properties["FallSpeed"];

                var entry = Instantiate(roomPrefab, container);
                entry.SetPanel(rulePanel);
                entry.RoomName = room.Name;
                entry.Settings = settings;

                instancedRoomEntries.Add(entry);
            }

            fetchingPanel.SetActive(false);
        }
    }
}
