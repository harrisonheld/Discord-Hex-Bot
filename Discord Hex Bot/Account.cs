using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace Discord_Hex_Bot
{
    public class Account
    {
        const string ACCOUNTS_PATH = @"./accounts.csv"; // .txt where accounts are stored
        const string DELIMTIER = ",";

        // things that should be saved
        public ulong accountId { get; set; } // this should be the same as the discord user's ID
        public int coins { get; set; } = 0;

        // things that should not be saved
        private AccountStatus status = AccountStatus.DoingNothing;

        public Account(ulong _accountId)
        {
            accountId = _accountId;

            // check if this id is already in the accounts list
            string[] lines = File.ReadAllLines(ACCOUNTS_PATH);

            // for all lines, except the first (which is the header)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split(DELIMTIER);

                ulong lineUserId = ulong.Parse(fields[0]);

                // if user was found
                if (lineUserId == accountId)
                {
                    coins = int.Parse(fields[1]);

                    break;
                }
            }
        }

        public void Save()
        {

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"User ID: {accountId}\n");
            sb.Append($"Coins: {coins}\n");
            return sb.ToString();
        }
    }

    public enum AccountStatus
    {
        DoingNothing,
        InLobby,
    }
}
