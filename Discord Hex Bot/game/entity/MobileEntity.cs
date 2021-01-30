using Discord_Hex_Bot.game.math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot.game.entity
{
    abstract class MobileEntity : Entity
    {
        public override bool move(Direction direction)
        {
            if (this.board.isClear(this.pos.offset(direction), this.layer))
            {
                base.move(direction);
                return true;
            }
            else return false;
        }
    }
}
