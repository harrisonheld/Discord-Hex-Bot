using System;
using System.Collections.Generic;

namespace Discord_Hex_Bot.game
{
    public class GameInstance
    {
        public static GameInstance INSTANCE;

        public List<entity.Entity> entities = new List<entity.Entity> { };
        public List<entity.Player> players = new List<entity.Player> { };
        private Random random;

        public render.Board board;

        public GameInstance(List<entity.Player> entities)
        {
            foreach (entity.Player player in entities)
            {
                this.entities.Add(player);
            }
            this.board = new render.Board(this);
            this.players = entities;
            INSTANCE = this;
            this.BroadcastToAll("pingaz");
        }
        public GameInstance(List<entity.Entity> entities, int seed)
        {
            this.entities = entities;
        }
        public void HandleInput(string[] args, entity.Player sender)
        {
            // log the args
            for(int i = 0; i < args.Length; i++)
            {
                Console.WriteLine($"{i}: {args[i]}");
            }

            if(entities.Contains(sender))
            {

            }
        }

        public void Spawn(entity.Entity entity)
        {
            this.entities.Add(entity);
            this.Redraw();
        }

        private void Redraw()
        {
            throw new NotImplementedException();
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
    }
}
