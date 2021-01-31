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
            return !this.GetEntity(position, layer).Immovable();
        }


    }
}