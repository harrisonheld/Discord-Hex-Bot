namespace Discord_Hex_Bot.game.entity
{
    class Rock : Entity
    {
        public Rock(GameInstance game, math.Position position) : base(game)
        {
            this.pos = position;
            this.glyph = Settings.ROCK_GLYPHS[this.game.random.Next(Settings.ROCK_GLYPHS.Length)];
        }
        public override bool Immovable()
        {
            return true;
        }
    }
}
