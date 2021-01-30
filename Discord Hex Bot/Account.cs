using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace Discord_Hex_Bot
{
    public class Account
    {
        const string ACCOUNTS_PATH = @"./accounts.txt"; // .txt where accounts are stored
        const string DELIMTIER = ",";

        public ulong userId { get; set; } // this should be the same as the discord user's ID
        public int coins;

        private ulong channelId;
        private ulong guildId;

        public Account(ulong _userId)
        {
            userId = _userId;

            // check if this id is already in the accounts list
            string[] lines = File.ReadAllLines(ACCOUNTS_PATH);
            bool foundUser = false; // if the account was found in the accounts list

            // for all lines, except the first (which is the header)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split(DELIMTIER);

                ulong lineUserId = ulong.Parse(fields[0]);

                // if user was found
                if (lineUserId == userId)
                {
                    foundUser = true;

                    coins = int.Parse(fields[1]);
                }
            }

            // if the user wasnt found in the list, make a new one
            if(!foundUser)
            {
                coins = 0;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"User ID: {userId}\n");
            sb.Append($"Coins: {coins}\n");
            return sb.ToString();
        }
    }
}
