using Discord_Hex_Bot.game.entity;
using Discord_Hex_Bot.game.math;
using Discord_Hex_Bot.game.render;
using System.Collections.Generic;

namespace Discord_Hex_Bot.game
{
    public class Tile
    {
        public Position position;
        public SortedDictionary<RenderLayer, Entity> layers;
        public char texture;

        internal Entity getEntity(RenderLayer layer)
        {
            return layers[layer];
        }
    }
}