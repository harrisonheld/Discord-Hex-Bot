using System.IO;
using System;

namespace Discord_Hex_Bot.game.entity
{
    public class Player : MobileEntity, IComparable
    {
        private readonly UserInfo userInfo;
        public bool turn;

        public UserInfo Info
        {
            get
            {
                return this.userInfo;
            }
        }

        public Player(GameInstance game, math.Position position, UserInfo info) : base(game, position)
        {
            this.turn = false;
            this.userInfo = info;
            this.dirty = true;
            this.glyph = Program.UserIdToUsername(this.Info.UserId)[0];

            // check if this id is already in the accounts list
            string[] lines = File.ReadAllLines(Settings.PLAYERS_PATH);

            // for all lines, except the first (which is the header)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split(Settings.CSV_DELIMITER);

                ulong lineUserId = ulong.Parse(fields[0]);

                // if user was found
                if (lineUserId == this.userInfo.UserId)
                {
                    break;
                }
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Player player = obj as Player;
            if (player != null)
                return this.Info.UserId.CompareTo(player.Info.UserId);
            else
                throw new ArgumentException("Object is not a Player");
        }

        public void Shoot(math.Direction direction)
        {
            Bullet bullet = new Bullet(this.game, this.pos, this, direction);
            this.game.Spawn(bullet);
        }
    }
}
