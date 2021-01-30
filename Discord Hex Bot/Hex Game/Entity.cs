using System;
using System.Drawing;

using Discord_Hex_Bot;

namespace Hex_Game
{
    public class Entity
    {
        private Position pos;
        private bool dirty;

        public void move(Direction direction)
        {
            switch(direction)
            {
                case UP:
                    this.pos.y += 1;
                    break;
                case DOWN:
                    this.pos.y -= 1;
                    break;
                case LEFT:
                    this.pos.x -= 1;
                    break;
                case RIGHT:
                    this.pos.x += 1;
                    break;
            }
        }

    }
}