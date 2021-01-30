﻿using Discord;
using Discord.WebSocket;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Discord_Hex_Bot
{
    class Program
    {
        // the prefix used to do commands
        private const string PREFIX = "hex.";
        private int prefixLength;

        // what users are talking
        List<ulong> userIDs = new List<ulong>();

        IMessageChannel dmingChannel;

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

            //Commands begin here
            if (command.Equals("start"))
            {
                // do something
            }

            return Task.CompletedTask;
        }
    }
}
