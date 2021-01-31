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

        public const int PLAYER_COUNT = 2; // how many players are needed to play

        public static char[] GROUND_GLYPHS = new char[] { ' ' };
        public static char[] ROCK_GLYPHS = new char[] { '@' };

        // isaiah and harrisons account ids, in that order
        public static ulong[] CREATOR_IDS = new ulong[] { 328227485013639168, 741448086701867060 };

        public static int MAP_HEIGHT = 18;
        public static int MAP_WIDTH = MAP_HEIGHT * 5;

        public static Dictionary<Emoji, EmojiWord> EMOJI_ARGS_DICT = new Dictionary<Emoji, EmojiWord>()
        {
            [new Emoji("🏹")] = EmojiWord.Bow,

            [new Emoji("⬅️")] = EmojiWord.Left,
            [new Emoji("➡️")] = EmojiWord.Right,
            [new Emoji("⬆️")] = EmojiWord.Up,
            [new Emoji("⬇️")] = EmojiWord.Down,
        };

        public static double ROCK_FREQUENCY = .015D;
    }

    public enum EmojiWord
    {
        Bow, 

        Left,
        Right,
        Up,
        Down
    }
}