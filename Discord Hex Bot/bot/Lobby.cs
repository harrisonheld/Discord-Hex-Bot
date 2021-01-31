using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

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
            instance = new GameInstance(ref users);
            status = LobbyStatus.InGame;
        }
        public void SuspendGame(UserInfo info)
        {
            if (instance == null)
                return;

            instance.End(info);
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
                    if (users.Count < Settings.MAX_PLAYERS)
                        SuspendGame(users[idx]);

                    instance.getPlayerFromInfo(users[idx]).Remove();
                }

                users.RemoveAt(idx);
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

            Console.WriteLine($"Could not get user with id {userId}. This may cause errors.");
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
        public void HandleInput(string[] args, ulong userId)
        {
            Console.WriteLine("Lobby is doing Handle Input");
            instance.handleCommand(args, GetUserInfoById(userId));
        }

        public EmbedBuilder LobbyInfoEmbed(UserInfo whoIsEmbedFor)
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

            string mentionOfWhoEmbedIsFor = Program.UserIdToMention(whoIsEmbedFor.UserId);

            // get a fun fact about hexagons
            string[] splashLines = File.ReadAllLines(Settings.SPLASH_TEXT_PATH);
            int splashIdx = Program.rand.Next(0, splashLines.Length);
            string splashText = splashLines[splashIdx];

            return new EmbedBuilder()
            {
                Title = name,
                Description = $"You're in this lobby, {mentionOfWhoEmbedIsFor}.",
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
                Footer = new EmbedFooterBuilder()
                {
                    IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/99/Hexagon.svg/693px-Hexagon.svg.png",
                    Text = $"Did you know?\n{splashText}"
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
