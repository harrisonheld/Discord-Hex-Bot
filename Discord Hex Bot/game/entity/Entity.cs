using Discord_Hex_Bot.game.math;
using Discord_Hex_Bot.game.render;

namespace Discord_Hex_Bot.game.entity
{
    public class Entity
    {

        public static Entity EMPTY = new Entity();

        public Position pos;
        // if this is true, the renderer should refresh this entity
        public bool dirty;
        public RenderLayer layer;
        public Board board;
        // lol texture
        public char glyph;

        public Entity()
        {
            this.pos = new Position(-1, -1);
            this.dirty = false;
            this.board = Board.INVALID;
            this.layer = RenderLayer.Background;
            this.glyph = Settings.GROUND_GLYPHS[0];
        }
        private Entity(Position _position, RenderLayer _renderLayer, Board _board)
        {
            this.pos = _position;
            this.dirty = true;
            this.layer = _renderLayer;
            this.board = _board;
        }

        public virtual bool Immovable()
        {
            return false;
        }

        public virtual bool Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    pos.Y += 1;
                    break;
                case Direction.South:
                    pos.Y -= 1;
                    break;
                case Direction.East:
                    pos.X -= 1;
                    break;
                case Direction.West:
                    pos.X += 1;
                    break;
            }
            return true;
        }

        public virtual void Step()
        {

        }

        public void Remove()
        {
            if(this.board.room.entities.Contains(this))
            {
                this.board.room.entities.Remove(this);
            }
        }

    }
}