﻿using Discord_Hex_Bot.game.math;

namespace Discord_Hex_Bot.game.entity
{
    public abstract class MobileEntity : Entity
    {
        public MobileEntity(GameInstance game, Position position) : base(game)
        {
            this.pos = position;
        }
     
        public override bool Move(Direction direction)
        {
            if (this.board.IsClear(this.pos.offset(direction), this.layer))
            {
                base.Move(direction);
                return true;
            }
            else return false;
        }
    }
}
