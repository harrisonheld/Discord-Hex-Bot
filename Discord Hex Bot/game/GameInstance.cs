using Discord_Hex_Bot.game.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot.game
{
    public class GameInstance
    {
        public static GameInstance INSTANCE;

        public List<entity.Entity> entities = new List<entity.Entity> { };
        public List<entity.Player> players = new List<entity.Player> { };
        public Random random;
        public bool active;
        public int steps;

        public render.Board board;

        public GameInstance(List<UserInfo> userInfos)
        {
            INSTANCE = this;
            this.random = new Random();
            this.active = true;
            this.steps = 0;
            for (int i = 0; i < Settings.MAP_HEIGHT * Settings.MAP_WIDTH; i++)
            {
                this.entities.Add(new entity.Entity(this));
            }
            foreach (UserInfo info in userInfos)
            {
                this.players.Add(new entity.Player(this, new math.Position(this.random.Next(Settings.MAP_WIDTH), this.random.Next(Settings.MAP_HEIGHT)), info));
            }
            foreach (entity.Player player in this.players)
            {
                int index = player.pos.X * 10 + player.pos.Y;
                this.entities[index] = player;
            }
            this.board = new render.Board(this);    
            this.BroadcastToAll("Lobby full- game has begun!");

            this.players[this.steps % this.players.Count].turn = true;
        }

        public void Step()
        {
            foreach (entity.Entity entity in entities)
            {
                entity.Step();
            }
            this.steps++;
            List<ulong> channels = new List<ulong> { };
            List<UserInfo> infoList = new List<UserInfo> { };
            foreach (Player player in this.players)
            {
                if (!channels.Contains(player.Info.ChannelId))
                {
                    infoList.Add(player.Info);
                    channels.Add(player.Info.ChannelId);
                }
                player.turn = false;
            }
            foreach(UserInfo info in infoList) { 
                Program.ShowRenderToUser(info, this.GetMap());
            }
            this.players[this.steps % this.players.Count].turn = true;
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
            int index = entity.pos.X * 10 + entity.pos.Y;
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

        internal void End(entity.Player leaver)
        {
            string name = Program.UserIdToUsername(leaver.Info.UserId);
            this.BroadcastToAll(name);
            this.active = false;
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

        public string[] GetMap()
        {
            StringBuilder charBuf = new StringBuilder();
            foreach (Entity entity in this.entities)
            {
                charBuf.Append(entity.glyph);
            }
            string all = charBuf.ToString();
            string[] strs = new string[Settings.MAP_HEIGHT];
            for (int i = 0; i < Settings.MAP_HEIGHT; i++)
            {
                strs[i] = all.Substring(Settings.MAP_WIDTH * i, Settings.MAP_WIDTH);
            }
            return strs;
        }

    }

}
