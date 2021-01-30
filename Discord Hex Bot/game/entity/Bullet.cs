namespace Discord_Hex_Bot.game.entity
{
    public class Bullet : MobileEntity
    {
        public Bullet(Entity source, math.Direction direction)
        {
            this.layer = render.RenderLayer.Main;
            if(!source.board.isClear(source.pos.offset(direction), this.layer))
            {
                this.Remove();
            }
        }
        public math.Direction direction;
        public override void Step()
        {
            this.pos = this.pos.offset(this.direction);
            if(!this.board.isClear(this.pos, this.layer))
            {
                // if(this.board.getEntity(this.pos, render.RenderLayer.Main) instanceof ())
                this.Remove();
            }
        }
    }
}
