using System.Collections.Generic;
using System.Text;
using Discord_Hex_Bot.game;
using Discord_Hex_Bot.game.entity;

namespace Discord_Hex_Bot
{
    class Lobby
    {
        private List<Player> players = new List<Player>();
        public List<Player> Players 
        {
            get
            {
                return players;
            }
        }
        private GameInstance instance;

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
            instance = new GameInstance(players);
        }
        public void SuspendGame()
        {
            status = LobbyStatus.Waiting;
        }

        public void AddPlayer(UserInfo info)
        {
            Player p = new Player(info);
            players.Add(p);

            if (players.Count >= Settings.MAX_PLAYERS)
                StartGame();
        }
        public bool RemovePlayerById(ulong id)
        {
            int idx = IdToIdx(id);
            if (idx > 0)
            {
                players.RemoveAt(idx);

                if (players.Count < Settings.MAX_PLAYERS)
                    SuspendGame();

                return true;
            }

            return false;
        }
        public Player GetPlayerById(ulong id)
        {
            foreach (Player p in players)
            {
                if (p.Id == id)
                    return p;
            }

            return null;
        }
        public bool ContainsPlayerWithId(ulong id)
        {
            foreach (Player p in players)
            {
                if (p.Id == id)
                    return true;
            }

            return false;
        }
        private int IdToIdx(ulong id)
        {
            int idx = 0;

            foreach (Player p in players)
            {
                if (p.Id == id)
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
            instance.HandleInput(args, GetPlayerById(id));
        }

        public string LobbyInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Players: {players.Count} / {Settings.MAX_PLAYERS}\n");
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
