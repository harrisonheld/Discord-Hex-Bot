using Discord_Hex_Bot.game.math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot.game.entity
{
    public abstract class MobileEntity : Entity
    {
        public override bool Move(Direction direction)
        {
            if (this.board.isClear(this.pos.offset(direction), this.layer))
            {
                base.Move(direction);
                return true;
            }
            else return false;
        }
    }
}
