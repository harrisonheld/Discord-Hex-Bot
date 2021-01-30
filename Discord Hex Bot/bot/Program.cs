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
                if (LobbyManager.ContainsPlayerWithId(authorId))
                {
                    Player p = LobbyManager.GetLobbyContainingPlayerId(authorId).GetPlayerById(authorId);

                    ulong joinedFromGuildId = p.Info.GuildId;
                    ulong joinedFromChannelId = p.Info.ChannelId;

                    IGuild guild = _client.GetGuild(joinedFromGuildId);
                    string joinedFromGuildName = guild.Name;
                    string joinedFromChannelName = guild.GetChannelAsync(joinedFromChannelId).Result.Name;

                    message.Channel.SendMessageAsync($"You are already in a lobby! You joined from " +
                        $"the channel #{joinedFromChannelName} in the server {joinedFromGuildName}\n" +
                        $"Use hex.leavelobby to close that session if you want to start a new one here.");

                    return Task.CompletedTask;
                }

                Lobby lobby = LobbyManager.AssignPlayerToLobby(info);

                string title = "";
                if (lobby.Players.Count == Settings.MAX_PLAYERS)
                    title = "The game can start now!";
                else
                    title = "You have joined a lobby!";

                EmbedBuilder eb = new EmbedBuilder()
                {
                    Title = title,
                    Description = "Here's some stats:",
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Players: ",
                            Value = $"[{lobby.Players.Count} / {Settings.MAX_PLAYERS}]",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Status:",
                            Value = lobby.Status,
                            IsInline = true
                        }
                    },

                    Color = Color.Blue
                };

                message.Channel.SendMessageAsync("", false, eb.Build());
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
                if(!LobbyManager.ContainsPlayerWithId(authorId))
                {
                    message.Channel.SendMessageAsync("You aren't in a lobby!");
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

            else if (command.Equals("unicodetest"))
            {
                StringBuilder sb = new StringBuilder();
                int start = int.Parse(message.Content.Split(" ")[1]);
                int end = start + 900;
                message.Channel.SendMessageAsync($"Printing chars {start}-{end}");

                for(int i = start; i < end; i++)
                {
                    sb.Append(Convert.ToChar(i) + " ");

                    if (i % 20 == 0)
                        sb.Append("\n");
                }

                message.Channel.SendMessageAsync(sb.ToString());
            }

            return Task.CompletedTask;
        }

        public static Task BroadcastToUser(UserInfo info, string text)
        {
            ulong channelId = info.ChannelId;
            ulong guildId = info.GuildId;

            IGuild guild = _client.GetGuild(guildId);
            // the following will break if its a dm channel, probably
            ISocketMessageChannel channel = guild.GetChannelAsync(channelId).Result as ISocketMessageChannel;

            channel.SendMessageAsync(text);

            return Task.CompletedTask;
        }
    }
}
