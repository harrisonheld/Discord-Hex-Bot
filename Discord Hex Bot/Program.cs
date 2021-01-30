using Discord;
using Discord.WebSocket;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot
{
    class Program
    {
        // the prefix used to do commands
        private const string PREFIX = "hex.";
        private int prefixLength;

        public static void Main()
        => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        public async Task MainAsync()
        {
            prefixLength = PREFIX.Length;

            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;
            _client.Log += Log;

            var token = File.ReadAllText(@"./token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task CommandHandler(SocketMessage message)
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

            //Commands begin here
            if (command.Equals("joinlobby"))
            {
                if (LobbyManager.ContainsPlayerWithId(authorId))
                {
                    message.Channel.SendMessageAsync("You are already in a lobby!");
                    return Task.CompletedTask;
                }

                Lobby l = LobbyManager.AssignPlayerToLobbyById(authorId);

                StringBuilder sb = new StringBuilder();
                sb.Append("You have joined a lobby\n");
                sb.Append(l.LobbyInfo());

                message.Channel.SendMessageAsync(sb.ToString());
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
            else if (command.Equals("embedtest"))
            {
                message.Channel.SendMessageAsync("Doing embed test.");

                var eb = new EmbedBuilder()
                {
                    Title = "This is the title.",
                    Footer = new EmbedFooterBuilder()
                    {
                        Text = "This is the footer text.",
                        IconUrl = "https://cdn.discordapp.com/attachments/741747309683015860/805149799199670272/df6bb81a4cfdff3507e261ebaf6a40efe7bf225e223a74a9d824b755d0fb1e46_1.jpg.jpg"
                    },
                    Description = "This is the description",
                    Fields = new List<EmbedFieldBuilder>()
                    {
                        new EmbedFieldBuilder()
                        {
                            Name = "Field 1 (inline)",
                            Value = "Value 1",
                            IsInline = true
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Field 2",
                            Value = "Value 2",
                            IsInline = false
                        },
                        new EmbedFieldBuilder()
                        {
                            Name = "Field 3 (inline)",
                            Value = "Value 3",
                            IsInline = true
                        }
                    },
                    ThumbnailUrl = "https://media.discordapp.net/attachments/741747309683015860/804922847235407912/streamer_man.png",
                    Color = new Color(0, 0, 0),
                };

                message.Channel.SendMessageAsync("", false, eb.Build());
            }

            return Task.CompletedTask;
        }
    }
}
