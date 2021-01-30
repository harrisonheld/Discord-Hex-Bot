namespace Discord_Hex_Bot.game.entity
{
    class Rock : Entity
    {
        public Rock(math.Position position, render.Board board)
        {
            this.pos = position;
            this.board = board;
            this.layer = render.RenderLayer.Main;
        }
        public override bool Immovable()
        {
            return true;
        }
    }
}
