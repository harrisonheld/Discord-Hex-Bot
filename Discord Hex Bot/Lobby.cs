using System;
using System.Collections.Generic;
using System.Text;

using Discord_Hex_Bot.game.entity;

namespace Discord_Hex_Bot
{
    class Lobby
    {
        private const int MAX_PLAYERS = 2; // how many players are needed to play
        private List<Player> players = new List<Player>();
        public List<Player> Players 
        {
            get
            {
                return players;
            }
        }

        private LobbyStatus status = LobbyStatus.Waiting;
        public LobbyStatus Status
        {
            get
            {
                return status;
            }
        }

        public void StartGame()
        {
            status = LobbyStatus.InGame;
        }
        public void SuspendGame()
        {
            status = LobbyStatus.Waiting;
        }

        public void AddAccountById(ulong id)
        {
            Player p = new Player(id);
            players.Add(p);

            if (players.Count >= MAX_PLAYERS)
                StartGame();
        }
        public bool RemoveAccountById(ulong id)
        {
            int idx = IdToIdx(id);
            if (idx > 0)
            {
                players.RemoveAt(idx);

                if (player.Count < MAX_PLAYERS)
                    SuspendGame();

                return true;
            }

            return false;
        }
        public Player GetAccountById(ulong id)
        {
            foreach (Player p in players)
            {
                if (p.playerId == id)
                    return p;
            }

            return null;
        }
        public bool ContainsAccountWithId(ulong id)
        {
            foreach (Player p in players)
            {
                if (p.playerId == id)
                    return true;
            }

            return false;
        }
        private int IdToIdx(ulong id)
        {
            int idx = 0;

            foreach (Player p in players)
            {
                if (p.playerId == id)
                    return idx;

                idx++;
            }

            return -1;
        }

        /// <summary>
        /// ISAIAH, USE THIS ONE
        /// </summary>
        /// <param name="args"></param>
        /// <param name="id"></param>
        public void AcceptCommandFromId(string[] args, ulong id)
        {

        }

        public string LobbyInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Players: {players.Count} / {MAX_PLAYERS}\n");
            sb.Append($"Status: {status}\n");
            return sb.ToString();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Players: {players.Count}\n");
            sb.Append($"Status: {status}\n");
            return sb.ToString();
        }
    }

    public enum LobbyStatus
    { 
        Waiting,
        InGame,
    }
}
