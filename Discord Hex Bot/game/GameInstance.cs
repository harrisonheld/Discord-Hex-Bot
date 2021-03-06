﻿using Discord_Hex_Bot.game.entity;
using System;
using System.Collections.Generic;
using System.Text;
using Discord_Hex_Bot.game.math;

namespace Discord_Hex_Bot.game
{
    public class GameInstance
    {
        public static GameInstance INSTANCE;

        private List<UserInfo> infosFromLobby;

        public List<entity.Entity> entities = new List<entity.Entity> { };
        public List<entity.Player> players = new List<entity.Player> { };
        public Random random;
        public bool active;
        public int steps;

        public GameInstance(ref List<UserInfo> infosFromLobby)
        {
            this.infosFromLobby = infosFromLobby;

            INSTANCE = this;
            this.random = new Random();
            this.active = true;
            this.steps = 0;
            for (int i = 0; i < Settings.MAP_HEIGHT * Settings.MAP_WIDTH; i++)
            {
                if(random.NextDouble() < Settings.ROCK_FREQUENCY)
                {
                    this.entities.Add(new Rock(this, new Position(i & Settings.MAP_WIDTH, i / Settings.MAP_WIDTH)));
                } else
                {
                    this.entities.Add(new Entity(this));
                }
            }
            foreach (UserInfo info in infosFromLobby)
            {
                this.players.Add(new entity.Player(this, new math.Position(this.random.Next(Settings.MAP_WIDTH), this.random.Next(Settings.MAP_HEIGHT)), info));
            }
            foreach (entity.Player player in this.players)
            {
                int index = player.pos.getValue();
                this.entities[index] = player;
            }
            this.BroadcastToAll("Lobby full - game has begun!");
            RenderMapToAll();

            this.players[this.steps % this.players.Count].turn = true;
        }

        public void Step()
        {
            for(int i = 0; i < this.entities.Count; i++)
            {
                entities[i].Step();
            }
            this.steps++;
            List<ulong> channels = new List<ulong> { };
            List<UserInfo> infoList = new List<UserInfo> { };

            RenderMapToAll();

            foreach (Player player in this.players)
            {
                if (!channels.Contains(player.Info.ChannelId))
                {
                    infoList.Add(player.Info);
                    channels.Add(player.Info.ChannelId);
                }
                player.turn = false;
            }

            this.players[this.steps % this.players.Count].turn = true;
        }

        public void RenderMapToAll()
        {
            // for each player
            for (int i = 0; i < infosFromLobby.Count; i++)
            {
                UserInfo editedInfo = infosFromLobby[i];
                // render the map and assign MessageReactIds
                editedInfo.ReactMessageId = (Program.ShowRenderToUser(infosFromLobby[i], this.GetMap()));
                infosFromLobby[i] = editedInfo;
            }
        }

        public void handleCommand(string[] args, UserInfo sender)
        {
            entity.Player player = this.getPlayerFromInfo(sender);

            foreach (entity.Player player1 in this.players)
            {
                if (player1.Info.UserId.Equals(sender.UserId))
                {
                    Console.WriteLine(player.pos.X.ToString() + " " + player.pos.Y.ToString());
                    io.HandleInput.handleInput(player.Info, args);
                }
            }
        }

        public void Spawn(entity.Entity entity)
        {
            int index = entity.pos.getValue();
            this.entities[index] = entity;
        }

        public void BroadcastToAll(String message)
        {
            List<UserInfo> uniqueUsers = new List<UserInfo> { };
            List<ulong> channels = new List<ulong> { };
            foreach (entity.Player player in this.players)
            {
                ulong channelId = player.Info.ChannelId;
                if(!channels.Contains(channelId))
                {
                    uniqueUsers.Add(player.Info);
                    channels.Add(channelId);
                }
            }
            foreach (UserInfo info in uniqueUsers)
            {
                Program.BroadcastToUser(info, message);
            }
        }

        public void End(entity.Player leaver)
        {
            string mention = Program.UserIdToMention(leaver.Info.UserId);
            string message = "The game is over! " + mention + " was shot down!";
            this.BroadcastToAll(mention + " left the game.");
            this.active = false;
            Program.EndGame(this, leaver.Info);
        }

        public entity.Player getPlayerFromInfo(UserInfo info)
        {
            foreach (entity.Player player in this.players)
            {
                if(player.Info.UserId.Equals(info.UserId))
                {
                    return player;
                }
            }
            Console.WriteLine("No player was found in GameInstance.getPlayerFromInfo().");
            return new entity.Player(this, new math.Position(-1, -1), info);
        }

        public string[] GetMap()
        {
            StringBuilder charBuf = new StringBuilder();
            foreach (Entity entity in this.entities)
            {
                charBuf.Append(entity.glyph);
                // debug
                // charBuf.Append((this.entities.IndexOf(entity) & 10).ToString()[0]);
            }
            string all = charBuf.ToString();
            string[] strs = new string[Settings.MAP_HEIGHT];
            for (int i = 0; i < Settings.MAP_HEIGHT; i++)
            {
                strs[i] = all.Substring(Settings.MAP_WIDTH * i, Settings.MAP_WIDTH);
            }
            return strs;
        }

        public Entity GetEntity(Position position)
        {
            if (position.X < 0 || position.Y < 0 || position.X > Settings.MAP_WIDTH || position.Y > Settings.MAP_HEIGHT)
            {
                return new Rock(this, position);
            }
                return this.entities[position.getValue()];
        }

        public bool IsClear(Position position)
        {
            if (position.X < 0 || position.Y < 0)
            {
                return false;
            }
            return !this.GetEntity(position).Immovable();
        }
    }

}
