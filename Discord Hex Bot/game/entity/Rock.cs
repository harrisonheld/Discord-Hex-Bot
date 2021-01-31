namespace Discord_Hex_Bot.game.entity
{
    class Rock : Entity
    {
        public Rock(math.Position position, render.Board board)
        {
            this.pos = position;
            this.board = board;
            this.layer = render.RenderLayer.Main;
            this.glyph = Settings.ROCK_GLYPHS[this.board.game.random.Next(Settings.ROCK_GLYPHS.Length)];
        }
        public override bool Immovable()
        {
            return true;
        }
    }
}
