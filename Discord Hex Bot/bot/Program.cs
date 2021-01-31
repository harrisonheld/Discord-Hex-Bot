using Discord;
using Discord.WebSocket;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using Discord_Hex_Bot.game;
using Discord_Hex_Bot.game.entity;

namespace Discord_Hex_Bot
{
    static class Program
    {
        // the prefix used to do commands
        private const string PREFIX = "hex.";
        private static int prefixLength;

        public static void Main()
        => MainAsync().GetAwaiter().GetResult();

        private static DiscordSocketClient _client;

        public static async Task MainAsync()
        {
            prefixLength = PREFIX.Length;

            _client = new DiscordSocketClient();
            _client.MessageReceived += HandleMessage;
            _client.Log += Log;
            _client.ReactionAdded += ReactionAdded;

            var token = File.ReadAllText(Settings.TOKEN_PATH);

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async static Task ReactionAdded(Cacheable<IUserMessage, UInt64> cachedMessage, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var message = await cachedMessage.GetOrDownloadAsync();

            Console.WriteLine((reaction.Emote as Emoji).Name);
        }

        private static Task HandleMessage(SocketMessage message)
        {
            //variables
            string command = "";
            int lengthOfCommand = -1;

            // if this is a direct message to the bot
            if(message.Channel.GetType() == typeof(SocketDMChannel))
            {
                
            }

            //filtering messages begin here
            if (!message.Content.StartsWith(PREFIX)) //This is your prefix
                return Task.CompletedTask;

            if (message.Author.IsBot) // This ignores all commands from bots
                return Task.CompletedTask;

            if (message.Content.Contains(' '))
                lengthOfCommand = message.Content.IndexOf(' ');
            else
                lengthOfCommand = message.Content.Length;

            // use Range operator 
            command = message.Content[prefixLength..lengthOfCommand].ToLower();

            ulong authorId = message.Author.Id;
            ulong channelId = message.Channel.Id;
            ulong guildId = (message.Channel as IGuildChannel).GuildId;
            UserInfo info = new UserInfo(authorId, channelId, guildId);

            //Commands begin here
            if (command.Equals("joinlobby"))
            {
                // if this user is already in a lobby
                if (LobbyManager.ContainsPlayerWithId(authorId))
                {
                    // make a message telling them where they joined the lobby from.
                    Lobby lobby = LobbyManager.GetLobbyContainingPlayerId(authorId);
                    UserInfo joinedFromUserInfo = lobby.GetUserInfoById(authorId);

                    ulong joinedFromGuildId = joinedFromUserInfo.GuildId;
                    ulong joinedFromChannelId = joinedFromUserInfo.ChannelId;

                    string joinedFromGuildName = _client.GetGuild(joinedFromGuildId).Name;
                    string joinedFromChannelName = (_client.GetChannel(joinedFromChannelId) as IMessageChannel).Name;

                    EmbedBuilder eb = lobby.LobbyInfoEmbed();

                    message.Channel.SendMessageAsync($"You are already in a lobby! You joined it from " +
                        $"the channel #{joinedFromChannelName} in the server {joinedFromGuildName}.\n" +
                        $"Use hex.leavelobby to close that session if you want to start a new one here.", false, eb.Build());
                }
                else
                {
                    // assign the player to a new lobby
                    Lobby lobby = LobbyManager.AssignPlayerToLobby(info);
                    EmbedBuilder eb = lobby.LobbyInfoEmbed();
                    message.Channel.SendMessageAsync("Joined a lobby!", false, eb.Build());
                }
            }
            else if (command.Equals("leavelobby"))
            {
                if (LobbyManager.ContainsPlayerWithId(authorId))
                {
                    LobbyManager.RemovePlayerById(authorId);
                    message.Channel.SendMessageAsync("You have left the lobby.");
                }
                else
                {
                    message.Channel.SendMessageAsync("You aren't in a lobby!");
                }
            }
            else if (command.Equals("input"))
            {
                // if the player isnt in a lobby
                if(!LobbyManager.ContainsPlayerWithId(authorId))
                {
                    message.Channel.SendMessageAsync("You aren't in a lobby!");
                    return Task.CompletedTask;
                }

                // if the player is in a lobby whos game hasnt started
                Lobby lobby = LobbyManager.GetLobbyContainingPlayerId(info.UserId);
                if (lobby.Status != LobbyStatus.InGame)
                {
                    message.Channel.SendMessageAsync("You cannot make inputs at this time. The lobby's game hasn't started yet.");
                    return Task.CompletedTask;
                }

                // make args list
                // remove first word, which is hex.input
                string[] parts = message.Content.Split(" ");
                string[] args = new string[parts.Length - 1];
                for(int i = 0; i < parts.Length - 1; i++)
                {
                    args[i] = parts[i + 1];
                    Console.WriteLine($"{i}: {args[i]}");
                }
                LobbyManager.AcceptCommandFromId(args, authorId);
            }
            else if( command.Equals("reacttest"))
            {
                message.Channel.SendMessageAsync("This is a test message. Try reacting to it.");
            }
            else if( command.Equals("rendertest"))
            {
                string[] lines = new string[18];
                for(int i = 0; i < lines.Length; i++)
                {
                    lines[i] = "abcabcabcabcabcabcabcabcapoopchugabcabcabcabcabcabcabcabcabcabcabcabcabcabc123456789012345";
                }

                message.Channel.SendMessageAsync("This is a 90*18 map.");
                ShowRenderToUser(info, lines);
            }

            return Task.CompletedTask;
        }

        public static void BroadcastToUser(UserInfo info, string text)
        {
            ulong channelId = info.ChannelId;

            // the following might break if its a dm channel
            ISocketMessageChannel channel = _client.GetChannel(channelId) as ISocketMessageChannel;
            IMessage message = channel.SendMessageAsync(text).Result;
        }
        public static void ShowRenderToUser(UserInfo info, string[] mapLines)
        {
            StringBuilder map = new StringBuilder();
            int width = mapLines[0].Length;
            int height = mapLines.Length;

            // opening tick marks
            map.Append("```");
            // top bar and corners
            map.Append("╔");
            for (int i = 0; i < width; i++)
                map.Append("═");
            map.Append("╗");
            map.Append("\n");

            // side bars, and map content
            foreach(string line in mapLines)
            {
                map.Append($"║{line}║\n");
            }

            // bottom bar and corners
            map.Append("╚");
            for (int i = 0; i < width; i++)
                map.Append("═");
            map.Append("╝");
            // closing tick marks
            map.Append("```");

            // send the map
            ulong channelId = info.ChannelId;
            ISocketMessageChannel channel = _client.GetChannel(channelId) as ISocketMessageChannel; // might break if dm channel, idk havent tried it
            IMessage message = channel.SendMessageAsync(map.ToString()).Result;

            // attach emojis
            foreach (string emojiString in Settings.EMOJI_STRINGS)
            {
                Emoji emoji = new Emoji(emojiString);
                message.AddReactionAsync(emoji);
            }
        }

        public static string UserIdToUsername(ulong userId)
        {
            return _client.GetUser(userId).Username;
        }
    }
}
