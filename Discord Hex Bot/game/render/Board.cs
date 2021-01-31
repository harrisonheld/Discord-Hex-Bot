using System.Collections.Generic;
using System.Text;
using Discord_Hex_Bot.game.entity;
using Discord_Hex_Bot.game.math;

namespace Discord_Hex_Bot.game.render
{
    public class Board
    {
        public GameInstance game;
        public Board(GameInstance game)
        {
            this.game = game;
        }


        public Entity GetEntity(Position position, RenderLayer layer)
        {
            if (position.X < 0 || position.Y < 0)
            {
                return new Rock(this.game, position);
            }
            return this.game.entities[position.X*10+position.Y];
        }

        public bool IsClear(Position position, RenderLayer layer)
        {
            if (position.X < 0 || position.Y < 0)
            {
                return false;
            }
            return this.GetEntity(position, layer).Equals(Entity.EMPTY);
        }

        public string[] GetMap()
        {
            StringBuilder charBuf = new StringBuilder();
            foreach(Entity entity in this.game.entities)
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