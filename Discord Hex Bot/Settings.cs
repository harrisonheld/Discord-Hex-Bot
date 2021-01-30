namespace Discord_Hex_Bot
{
    static class Settings
    {
        public const string PLAYERS_PATH = @"./players.csv";
        public const string CSV_DELIMITER = ",";
        public const int MAX_PLAYERS = 2; // how many players are needed to play

        public static char[] GROUND_GLYPHS = new char[3] {',', '`', '-'};
        public static char[] PLAYER_GLYPHS = new char[4] { 'a', 'b', 'c', 'd' };
        public static char[] ENEMY_GLYPHS = new char[1] { 'E' };
        public static char[] ARROW_GLYPHS = new char[2] { '/', '-' };
        public static char[] ROCK_GLYPHS = new char[1] { 'o' };
    }
}