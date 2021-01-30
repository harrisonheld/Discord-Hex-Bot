using Discord_Hex_Bot.game.render;
using System.IO;

namespace Discord_Hex_Bot.game.entity
{
    public class Player : MobileEntity
    {
        private readonly ulong id;
        private readonly ulong channelId;

        public ulong ChannelId
        {
            get
            {
                return this.channelId;
            }
        }
        public ulong Id
        {
            get
            {
                return this.id;
            }
        }

        public Player(ulong accountId, ulong channelId)
        {
            this.id = accountId;
            this.pos = new math.Position(0, 0);
            this.dirty = true;
            this.layer = RenderLayer.Main;

            // check if this id is already in the accounts list
            string[] lines = File.ReadAllLines(Settings.PLAYERS_PATH);

            // for all lines, except the first (which is the header)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = line.Split(Settings.CSV_DELIMITER);

                ulong lineUserId = ulong.Parse(fields[0]);

                // if user was found
                if (lineUserId == accountId)
                {

                    break;
                }
            }
        }

        public void Shoot(math.Direction direction)
        {
            Bullet bullet = new Bullet(this, direction);
            this.board.room.spawn(bullet);
        }
    }
}
