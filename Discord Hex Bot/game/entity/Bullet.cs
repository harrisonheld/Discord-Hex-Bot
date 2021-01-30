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
            this.pos = this.pos.offset(this.direction);
            if(!this.board.isClear(this.pos, this.layer))
            {
                // if(this.board.getEntity(this.pos, render.RenderLayer.Main) instanceof ())
                this.Remove()
            }
        }
    }
}
