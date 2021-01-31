namespace Discord_Hex_Bot.game.entity
{
    public class Bullet : MobileEntity
    {
        public Entity owner;

        public Bullet(GameInstance game, math.Position position, Entity source, math.Direction direction) : base(game, position)
        {
            this.owner = source;
            this.glyph = 'O';
            this.direction = direction;
            if(!source.game.IsClear(source.pos.offset(direction)))
            {
                this.Remove();
            }
        }
        public math.Direction direction;
        public override void Step()
        {
            this.Step(3);
        }

        public void Step(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!this.game.IsClear(this.pos.offset(this.direction)))
                { 
                    if(this.game.GetEntity(this.pos.offset(this.direction)) is Player)
                    {
                        this.game.GetEntity(this.pos.offset(this.direction)).Remove();
                    }
                    this.Remove();
                }
                this.Move(direction);
            }
        }
    }
}
