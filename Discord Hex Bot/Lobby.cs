using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot
{
    class Lobby
    {
        private const int MAX_PLAYERS = 2; // how many players are needed to play
        private List<Account> accounts = new List<Account>();
        public List<Account> Accounts 
        {
            get
            {
                return accounts;
            }
        }

        private LobbyStatus status = LobbyStatus.Waiting;
        public LobbyStatus Status
        {
            get
            {
                return status;
            }
        }

        public void StartGame()
        {
            status = LobbyStatus.InGame;
        }
        public void SuspendGame()
        {
            status = LobbyStatus.Waiting;
        }

        public void AddAccountById(ulong id)
        {
            Account a = new Account(id);
            accounts.Add(a);

            if (accounts.Count >= MAX_PLAYERS)
                StartGame();
        }
        public bool RemoveAccountById(ulong id)
        {
            int idx = IdToIdx(id);
            if (idx > 0)
            {
                accounts.RemoveAt(idx);

                if (accounts.Count < MAX_PLAYERS)
                    SuspendGame();

                return true;
            }

            return false;
        }
        public Account GetAccountById(ulong id)
        {
            foreach (Account a in accounts)
            {
                if (a.accountId == id)
                    return a;
            }

            return null;
        }
        public bool ContainsAccountWithId(ulong id)
        {
            foreach (Account a in accounts)
            {
                if (a.accountId == id)
                    return true;
            }

            return false;
        }
        private int IdToIdx(ulong id)
        {
            int idx = 0;

            foreach (Account a in accounts)
            {
                if (a.accountId == id)
                    return idx;

                idx++;
            }

            return -1;
        }

        /// <summary>
        /// ISAIAH, USE THIS ONE
        /// </summary>
        /// <param name="args"></param>
        /// <param name="id"></param>
        public void AcceptCommandFromId(string[] args, ulong id)
        {

        }

        public string LobbyInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Players: {accounts.Count} / {MAX_PLAYERS}\n");
            sb.Append($"Status: {status}\n");
            return sb.ToString();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Players: {accounts.Count}\n");
            sb.Append($"Status: {status}\n");
            return sb.ToString();
        }
    }

    public enum LobbyStatus
    { 
        Waiting,
        InGame,
    }
}
