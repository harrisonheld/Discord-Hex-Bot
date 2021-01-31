using System;
using System.Collections.Generic;
using System.Text;

using Discord_Hex_Bot.game;
using Discord_Hex_Bot.game.entity;

using Discord;

namespace Discord_Hex_Bot
{
    class Lobby
    {
        private List<UserInfo> users = new List<UserInfo>();
        private List<UserInfo> Users
        { 
            get
            {
                return users;
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
                instance.getPlayerFromInfo(users[idx]).Remove();
                users.RemoveAt(idx);

                if (users.Count < Settings.MAX_PLAYERS)
                    SuspendGame();

                return true;
            }

            return false;
        }
        public UserInfo GetUserInfoById(ulong userId)
        {
            foreach (UserInfo u in users)
            {
                if (u.UserId == userId)
                    return u;
            }

            Console.WriteLine("Cannot get user with id {0}. This may cause errors.");
            // just yolo it and return an empty user info
            return new UserInfo(0, 0, 0);
        }
        public bool ContainsPlayerWithId(ulong userId)
        {
            foreach (UserInfo u in users)
            {
                if (u.UserId == userId)
                    return true;
            }

            return false;
        }
        private int UserIdToIdx(ulong userId)
        {
            int idx = 0;

            foreach (UserInfo u in users)
            {
                if (u.UserId == userId)
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
        public void AcceptCommandFromId(string[] args, ulong userId)
        {
            instance.HandleInput(args, GetUserInfoById(userId));
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
                            Value = $"[{users.Count} / {Settings.MAX_PLAYERS}]",
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
            sb.Append($"Players: {users.Count}\n");
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
