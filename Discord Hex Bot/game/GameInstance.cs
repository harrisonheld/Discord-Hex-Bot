using System;
using System.Collections.Generic;

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
            this.random = new Random();

            this.active = true;
            INSTANCE = this;
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
                this.entities.Add(player);
            }
            this.board = new render.Board(this);    
            this.BroadcastToAll("PINGAZ YEEEAH BOY");

            this.players[this.steps % this.players.Count].turn = true;
        }

        public void Step()
        {
            this.players[this.steps % this.players.Count].turn = true;

            foreach (entity.Entity entity in entities)
            {
                entity.Step();
            }
            this.steps++;

            foreach (entity.Player player in this.players)
            {
                Program.ShowRenderToUser(player.Info, this.board.GetMap());
                player.turn = false;
            }
        }

        public void handleCommand(string[] args, UserInfo sender)
        {
            entity.Player player = this.getPlayerFromInfo(sender);

            foreach (entity.Player player1 in this.players)
            {
                if (player.Info.UserId.Equals(sender.UserId))
                {
                    Console.WriteLine("poggy");
                    io.HandleInput.handleInput(player.Info, args);
                }
            }
        }

        public void Spawn(entity.Entity entity)
        {
            this.entities.Add(entity);
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

    }
}
