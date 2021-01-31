﻿namespace Discord_Hex_Bot.game.entity
{
    public class Bullet : MobileEntity
    {
        public Entity owner;

        public Bullet(GameInstance game, math.Position position, Entity source, math.Direction direction) : base(game, position)
        {
            this.layer = render.RenderLayer.Main;
            this.pos = source.pos.offset(direction);
            this.owner = source;
            this.glyph = 'O';
            if(!source.game.IsClear(source.pos.offset(direction), this.layer))
            {
                this.Remove();
            }
        }
        public math.Direction direction;
        public override void Step()
        {
            this.pos = this.pos.offset(this.direction);
            if(!this.game.IsClear(this.pos, this.layer))
            {
                if(this.game.GetEntity(this.pos, render.RenderLayer.Main) is Player)
                this.Remove();
            }
        }
    }
}
