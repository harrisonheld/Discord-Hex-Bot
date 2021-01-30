using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot.game
{
    public class GameInstance
    {
        public List<entity.Player> players;
        private Random random;

        public GameInstance(List<entity.Player> _players)
        {
            this.players = _players;
        }
        public GameInstance(List<entity.Player> players, int seed)
        {
            this.players = players;
        }
        public void HandleInput(string[] args, entity.Player sender)
        {
            if(players.Contains(sender))
            {

            }
        }
    }
}
