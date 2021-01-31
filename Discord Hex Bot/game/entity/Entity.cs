using System;
using Discord_Hex_Bot.game.math;

namespace Discord_Hex_Bot.game.entity
{
    public class Entity
    {

        public static Entity EMPTY = new Entity(GameInstance.INSTANCE);

        public Position pos;
        // if this is true, the renderer should refresh this entity
        public bool dirty;
        public char glyph;
        public int id;
        public GameInstance game;


        public Entity(GameInstance game)
        {
            this.id = game.random.Next();
            this.pos = new Position(-1, -1);
            this.dirty = false;
            this.glyph = Settings.GROUND_GLYPHS[game.random.Next(0, Settings.GROUND_GLYPHS.Length)];
            this.game = game;
        }
        private Entity(Position _position)
        {
            this.pos = _position;
            this.dirty = true;
        }

        public virtual bool Immovable()
        {
            return false;
        }

        public virtual bool Move(Direction direction)
        {
            this.game.entities[this.pos.getValue()] = new Entity(this.game);
            this.pos = this.pos.offset(direction);
            this.game.entities[this.pos.getValue()] = this;
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