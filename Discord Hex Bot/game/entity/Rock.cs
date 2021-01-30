using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot.game.entity
{
    class Rock : Entity
    {
        public Rock(math.Position position, render.Board board)
        {
            this.pos = position;
            this.board = board;
        }
        public override bool Immovable()
        {
            return true;
        }
    }
}
