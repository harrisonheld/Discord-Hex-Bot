using System;
using Discord_Hex_Bot.game.math;
using Discord_Hex_Bot.game.render;

namespace Discord_Hex_Bot.game.entity
{
    public class Entity
    {

        public static Entity EMPTY = new Entity(GameInstance.INSTANCE);

        public Position pos;
        // if this is true, the renderer should refresh this entity
        public bool dirty;
        public RenderLayer layer;
        public Board board;
        public char glyph;
        public int id;
        public GameInstance game;


        public Entity(GameInstance game)
        {
            this.id = game.random.Next();
            this.pos = new Position(-1, -1);
            this.dirty = false;
            this.board = game.board;
            this.layer = RenderLayer.Background;
            this.glyph = Settings.GROUND_GLYPHS[game.random.Next(0, Settings.GROUND_GLYPHS.Length)];
            this.game = game;
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
            int index = this.pos.X * 10 + this.pos.Y;
            this.game.entities[index] = new Entity(this.game);
            this.pos = this.pos.offset(direction);
            int newIndex = this.pos.X * 10 + this.pos.Y;
            this.game.entities[newIndex] = this;
            return true;
        }

        public virtual void Step()
        {

        }

        public void Remove()
        {
            if(this.game.entities.Contains(this))
            {
                this.game.entities.Remove(this);
                if (this is Player)
                {
                    this.game.End((Player)this);
                }
            }
        }
    }
}