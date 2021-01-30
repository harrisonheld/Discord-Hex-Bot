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

        }
        public static Board INVALID = new Board(GameInstance.);

        public readonly GameInstance room;

        public SortedDictionary<Position, Tile> tiles = new SortedDictionary<Position, Tile> { };

        public Entity GetEntity(Position position, RenderLayer layer)
        {
            Tile tile = tiles[position];
            return tile.getEntity(layer);
        }

        public bool IsClear(Position position, RenderLayer layer)
        {
            return this.GetEntity(position, layer).Equals(Entity.EMPTY);
        }

    }
}