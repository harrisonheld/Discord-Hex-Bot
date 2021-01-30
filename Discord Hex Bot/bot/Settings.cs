﻿namespace Discord_Hex_Bot
{
    static class Settings
    {
        public const string PLAYERS_PATH = @"./resources/players.csv";
        public const string TOKEN_PATH = @"./resources/token.txt";

        public const string CSV_DELIMITER = ",";

        public const int MAX_PLAYERS = 2; // how many players are needed to play

        public static char[] GROUND_GLYPHS = new char[3] {',', '`', '-'};
        public static char[] PLAYER_GLYPHS = new char[4] { 'a', 'b', 'c', 'd' };
        public static char[] ENEMY_GLYPHS = new char[1] { 'E' };
        public static char[] ARROW_GLYPHS = new char[2] { '/', '-' };
        public static char[] ROCK_GLYPHS = new char[1] { 'o' };

        // harrison and isaiahs account ids, in that order
        public static ulong[] CREATOR_IDS = new ulong[2] { 741448086701867060, 328227485013639168 };
    }
}