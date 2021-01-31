using System.Collections.Generic;
using Discord_Hex_Bot.game.entity;
using Discord_Hex_Bot.game.math;

namespace Discord_Hex_Bot.game.render
{
    public class Board
    {
        public GameInstance game;
        public Board(GameInstance game)
        {
            this.game = game;
            for (int i = 0; i < Settings.MAP_HEIGHT; i++)
            {
                for (int j = 0; j < Settings.MAP_WIDTH; j++)
                {
                    this.entities.Add(new Position(j, i), new Entity(this.game));
                }
            }
        }
        public static Board INVALID = new Board(GameInstance.INSTANCE);


        public SortedDictionary<Position, Entity> entities = new SortedDictionary<Position, Entity> { };

        public Entity GetEntity(Position position, RenderLayer layer)
        {
            return entities[position];
        }

        public bool IsClear(Position position, RenderLayer layer)
        {
            return this.GetEntity(position, layer).Equals(Entity.EMPTY);
        }

    }
}