using System;
using System.Collections.Generic;
using Discord_Hex_Bot.game.entity;
using Discord_Hex_Bot.game.math;

namespace Discord_Hex_Bot.game.render
{
    public class Board
    {
        public static Board INVALID = new Board();

        public readonly Room room;

        public string texture;

        public SortedDictionary<Position, Tile> tiles;

        public Entity getEntity(Position position, RenderLayer layer)
        {
            Tile tile = tiles[position];
            return tile.getEntity(layer);
        }

        public bool isClear(Position position, RenderLayer layer)
        {
            return this.getEntity(position, layer).Equals(Entity.EMPTY);
        }

    }
}