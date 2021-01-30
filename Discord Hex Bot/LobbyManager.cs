using System;
using System.Collections.Generic;
using System.Text;
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
                if (lobby.ContainsAccountWithId(id))
                    return lobby;
            }

            return null;
        }
        public static Lobby AssignPlayerToLobbyById(ulong id)
        {
            foreach(Lobby lobby in lobbies)
            {
                if(lobby.Status == LobbyStatus.Waiting)
                {
                    lobby.AddAccountById(id);
                    return lobby;
                }
            }

            // if no lobbies were waiting for another player
            Lobby newLobby = CreateNewLobby();
            newLobby.AddAccountById(id);
            Debug.WriteLine("Created a new lobby.");

            return newLobby;
        }
        private static Lobby CreateNewLobby()
        {
            Lobby newLobby = new Lobby();
            lobbies.Add(newLobby);
            return newLobby;
        }
        public static void RemoveAccountById(ulong id)
        {
            foreach(Lobby lobby in lobbies)
            {
                if (lobby.RemoveAccountById(id))
                    return;
            }
        }
        public static bool ContainsAccountWithId(ulong id)
        { 
            foreach(Lobby lobby in lobbies)
            {
                if (lobby.ContainsAccountWithId(id))
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
