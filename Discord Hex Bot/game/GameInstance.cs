using System;
using System.Collections.Generic;

namespace Discord_Hex_Bot.game
{
    public class GameInstance
    {
        public List<entity.Entity> entities = new List<entity.Entity> { };
        private Random random;

        public GameInstance(List<entity.Player> entities)
        {
            foreach (entity.Player player in entities)
            {
                this.entities.Add(player);
            }
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

        public void spawn(entity.Entity entity)
        {
            this.entities.Add(entity);
            this.redraw();
        }

        private void redraw()
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
    }
}
