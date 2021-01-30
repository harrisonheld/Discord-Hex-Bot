using System.Collections.Generic;
using System.Diagnostics;

namespace Discord_Hex_Bot
{
    static class LobbyManager
    {
        public static List<Lobby> lobbies = new List<Lobby>();

        public static Lobby GetLobbyContainingPlayerId(ulong id)
        {
            foreach (Lobby lobby in lobbies)
            {
                if (lobby.ContainsPlayerWithId(id))
                    return lobby;
            }

            return null;
        }
        public static Lobby AssignPlayerToLobby(UserInfo info)
        {
            foreach(Lobby lobby in lobbies)
            {
                if(lobby.Status == LobbyStatus.Waiting)
                {
                    lobby.AddPlayer(info);
                    return lobby;
                }
            }

            // if no lobbies were waiting for another player
            Lobby newLobby = CreateNewLobby();
            newLobby.AddPlayer(info);
            Debug.WriteLine("Created a new lobby.");

            return newLobby;
        }
        private static Lobby CreateNewLobby()
        {
            Lobby newLobby = new Lobby();
            lobbies.Add(newLobby);
            return newLobby;
        }
        public static void RemovePlayerById(ulong id)
        {
            foreach(Lobby lobby in lobbies)
            {
                if (lobby.RemovePlayerById(id))
                    return;
            }
        }
        public static bool ContainsPlayerWithId(ulong id)
        { 
            foreach(Lobby lobby in lobbies)
            {
                if (lobby.ContainsPlayerWithId(id))
                    return true;
            }

            return false;
        }

        public static void AcceptCommandFromId(string[] args, ulong id)
        {
            Lobby lobby = GetLobbyContainingPlayerId(id);
            lobby.AcceptCommandFromId(args, id);
        }
    }
}