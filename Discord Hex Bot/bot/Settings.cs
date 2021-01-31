using System.Collections.Generic;

using Discord;

namespace Discord_Hex_Bot
{
    static class Settings
    {
        public const string PLAYERS_PATH = @"./resources/players.csv";
        public const string TOKEN_PATH = @"./resources/token.txt";
        public const string SPLASH_TEXT_PATH = @"./resources/hexFacts.txt";

        public const string CSV_DELIMITER = ",";

        public const int PLAYER_COUNT = 1; // how many players are needed to play

        public static char[] GROUND_GLYPHS = new char[] { '.' };
        public static char[] ROCK_GLYPHS = new char[] { '@' };

        // isaiah and harrisons account ids, in that order
        public static ulong[] CREATOR_IDS = new ulong[] { 328227485013639168, 741448086701867060 };

        public static int MAP_HEIGHT = 18;
        public static int MAP_WIDTH = MAP_HEIGHT * 5;

        public static Dictionary<Emoji, string[]> EMOJI_ARGS_DICT = new Dictionary<Emoji, string[]>()
        {
            // "⬅️", "➡️", "⬆️", "⬇️"
            [new Emoji("⬅️")] = new string[] { "move", "left" },
            [new Emoji("➡️")] = new string[] { "move", "right" },
            [new Emoji("⬆️")] = new string[] { "move", "up" },
            [new Emoji("⬇️")] = new string[] { "move", "down" },
        };

        public static double ROCK_FREQUENCY = .015D;
    }
}