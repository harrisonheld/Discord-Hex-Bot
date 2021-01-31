﻿using System.Collections.Generic;
using System.Text;

using Discord_Hex_Bot.game;
using Discord_Hex_Bot.game.entity;

using Discord;

namespace Discord_Hex_Bot
{
    class Lobby
    {
        private List<UserInfo> users = new List<UserInfo>();

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
            instance = new GameInstance(users);
        }
        public void SuspendGame()
        {
            status = LobbyStatus.Waiting;
        }

        public void AddPlayer(UserInfo info)
        {
            users.Add(info);

            if (users.Count >= Settings.MAX_PLAYERS)
            {
                StartGame();
            }
        }
        public bool RemovePlayerById(ulong id)
        {
            int idx = UserIdToIdx(id);
            if (idx >= 0)
            {
                // WARNING: the following line sucks, if it breaks something, use instance.RemovePlayer(players[idx]) instead
                // also make a new RemovePlayer(Player p)
                players[idx].Remove(); // removes player from the game
                players.RemoveAt(idx);

                if (players.Count < Settings.MAX_PLAYERS)
                    SuspendGame();

                return true;
            }

            return false;
        }
        public Player GetPlayerById(ulong userId)
        {
            foreach (Player p in players)
            {
                if (p.Info.UserId == userId)
                    return p;
            }

            return null;
        }
        public bool ContainsPlayerWithId(ulong userId)
        {
            foreach (Player p in players)
            {
                if (p.Info.UserId == userId)
                    return true;
            }

            return false;
        }
        private int UserIdToIdx(ulong userId)
        {
            int idx = 0;

            foreach (Player p in players)
            {
                if (p.Info.UserId == userId)
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

        public EmbedBuilder LobbyInfoEmbed()
        {
            return new EmbedBuilder()
            {
                Title = "Lobby",
                Description = "Here's some stats:",
                Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Players: ",
                            Value = $"[{players.Count} / {Settings.MAX_PLAYERS}]",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Status:",
                            Value = status,
                            IsInline = true
                        }
                    },

                Color = Color.Blue
            };
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
