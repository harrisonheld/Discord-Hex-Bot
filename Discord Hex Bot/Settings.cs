using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot
{
    static class Settings
    {
        public const string PLAYERS_PATH = @"./players.csv";
        public const string CSV_DELIMITER = ",";
        public const int MAX_PLAYERS = 2; // how many players are needed to play

        public const char GROUND_TEXTURE = 'o';
        public const char PLAYER_TEXTURE = 'o';
        public const char ENEMY_TEXTURE = 'o';
        public const char ARROW_TEXTURE = 'o';

    }
}