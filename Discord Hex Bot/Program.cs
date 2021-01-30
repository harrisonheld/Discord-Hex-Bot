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

            while(true)
            {
                string input = Console.ReadLine();
                if(input == "lobbies")
                {
                    foreach(Lobby lobby in LobbyManager.lobbies)
                    {
                        Console.WriteLine(lobby.ToString());
                    }    
                }
            }

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
                if (LobbyManager.ContainsAccountWithId(authorId))
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
            if (command.Equals("leavelobby"))
            {
                if (LobbyManager.ContainsAccountWithId(authorId))
                {
                    LobbyManager.RemoveAccountById(authorId);
                    message.Channel.SendMessageAsync("You have left the lobby.");
                }
                else
                {
                    message.Channel.SendMessageAsync("You aren't in a lobby!");
                }
            }
            if (command.Equals("input"))
            {
                if(LobbyManager.ContainsAccountWithId(authorId))
                {
                    message.Channel.SendMessageAsync("You aren't in a lobby!");
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

            return Task.CompletedTask;
        }
    }
}
