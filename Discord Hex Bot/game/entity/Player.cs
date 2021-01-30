using System;
using System.Collections.Generic;
using System.Text;
using Discord_Hex_Bot.game.render;

namespace Discord_Hex_Bot.game.entity
{
    class Player : MobileEntity
    {
        public ulong id;

        public Player(ulong _id, Board _board)
        {
            this.id = _id;
            this.layer = RenderLayer.Main;
            this.board = _board;
            this.dirty = true;
        }
    }
}
