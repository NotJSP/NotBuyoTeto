using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NotBuyoTeto.Ingame.MultiPlay {
    public class LobbyManager {
        public static readonly TypedLobby LeagueLobby = new TypedLobby("LeagueLobby", LobbyType.Default);
        public static readonly TypedLobby ClubLobby = new TypedLobby("ClubLobby", LobbyType.Default);
    }
}
