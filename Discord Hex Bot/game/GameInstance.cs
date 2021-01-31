﻿using System;
using System.Collections.Generic;

namespace Discord_Hex_Bot.game
{
    public class GameInstance
    {
        public static GameInstance INSTANCE;

        public List<entity.Entity> entities = new List<entity.Entity> { };
        public List<entity.Player> players = new List<entity.Player> { };
        public Random random;

        public render.Board board;

        public GameInstance(List<UserInfo> userInfos)
        {
            this.random = new Random();
            INSTANCE = this;
            foreach(UserInfo info in userInfos)
            {
                this.players.Add(new entity.Player(this, new math.Position(this.random.Next(Settings.MAP_WIDTH), this.random.Next(Settings.MAP_HEIGHT)), info));
            }
            foreach (entity.Player player in entities)
            {
                this.entities.Add(player);
            }
            this.board = new render.Board(this);
            this.BroadcastToAll("PINGAZ YEEEAH BOY");
        }

        public void Run()
        {
            foreach(entity.Player player in this.players)
            {

            }
        }

        public void HandleInput(string[] args, UserInfo sender)
        {
            entity.Player player = this.getPlayerFromInfo(sender);
            // log the args
            for(int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"{i}: {args[i]}");
            }

            if(entities.Contains(player))
            {

            }
        }

        public void Spawn(entity.Entity entity)
        {
            this.entities.Add(entity);
        }

        public void Step()
        {
            foreach (entity.Entity entity in entities)
            {
                entity.Step();
            }
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

        internal void End(entity.Player leaver)
        {
            string name = Program.UserIdToUsername(leaver.Info.UserId);
            this.BroadcastToAll(name);
        }

        public entity.Player getPlayerFromInfo(UserInfo info)
        {
            foreach (entity.Player player in this.players)
            {
                if(player.Info.Equals(info))
                {
                    return player;
                }
            }
            this.BroadcastToAll("No player found!");
            return new entity.Player(this, new math.Position(-1, -1), info);
        }

    }
}
