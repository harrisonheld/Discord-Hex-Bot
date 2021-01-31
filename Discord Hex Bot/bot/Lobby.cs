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
        private string name;
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

        public Lobby(string _name)
        {
            name = _name;
        }

        public void StartGame()
        {
            instance = new GameInstance(users);
            status = LobbyStatus.InGame;
        }
        public void SuspendGame()
        {
            if (instance == null)
                return;

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
                if(instance != null) // if game has been created yet
                {
                    instance.getPlayerFromInfo(users[idx]).Remove();
                }

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
            instance.handleCommand(args, GetUserInfoById(userId));
        }

        public EmbedBuilder LobbyInfoEmbed()
        {
            StringBuilder usernamesString = new StringBuilder();
            for(int i = 0; i < users.Count; i++)
            {
                // add the username to the string
                string username = Program.UserIdToUsername(users[i].UserId);
                usernamesString.Append(username);

                // add a comma after every username except the last one
                if (i != users.Count - 1)
                    usernamesString.Append(", ");
            }
            // append user count
            usernamesString.Append($" **[{users.Count} / {Settings.MAX_PLAYERS}]**");

            return new EmbedBuilder()
            {
                Title = name,
                Description = "Here's some stats:",
                Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Status:",
                            Value = status,
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Users:",
                            Value = usernamesString.ToString(),
                            IsInline = true
                        }
                    },

                Color = Color.Blue
            };
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Users: {users.Count}\n");
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
