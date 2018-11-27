using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace NotBuyoTeto.Ingame.MultiPlay.Club {
    public class RoomManager : MonoBehaviourPunCallbacks {
        private TypedLobby lobby = new TypedLobby("Club", LobbyType.SqlLobby);

        public RoomEntry[] Fetch() {
            PhotonNetwork.GetCustomRoomList(lobby, "");
            return null;
        }
    }
}
