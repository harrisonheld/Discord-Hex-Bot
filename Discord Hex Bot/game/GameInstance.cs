using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot.game
{
    public class GameInstance
    {
        public List<entity.Entity> entities;
        private Random random;

        public GameInstance(List<entity.Entity> entities)
        {
            this.entities = entities;
        }
        public GameInstance(List<entity.Entity> entities, int seed)
        {
            this.entities = entities;
        }
        public void HandleInput(string[] args, entity.Player sender)
        {
            if(entities.Contains(sender))
            {

            }
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
