using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot.game.entity
{
    public class Bullet : MobileEntity
    {
        public math.Direction direction;
        public override void Step()
        {
            if()
            this.pos = this.pos.offset(this.direction);
        }
    }
}
