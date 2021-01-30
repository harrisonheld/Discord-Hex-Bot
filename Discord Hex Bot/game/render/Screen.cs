using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot.game.render
{
    class Screen
    {

    }
    public enum RenderLayer
    {

        Background,
        // Board takes up these three layers
        Floor,
        Main,
        Air,
        // top layer reserved for dialog
        Top
    }
}
