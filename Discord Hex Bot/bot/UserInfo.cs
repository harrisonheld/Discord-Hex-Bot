using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Hex_Bot
{
    public struct UserInfo
    {
        public ulong UserId {get;set;}
        public ulong ChannelId { get; set; }
        public ulong GuildId { get; set; }

        public ulong ReactMessageId { get; set; }

        public void setReactMessageID(ulong id)
        {
            this.ReactMessageId = id;
        }
        public UserInfo(ulong _userId, ulong _channelId, ulong _guildId)
        {
            UserId = _userId;
            ChannelId = _channelId;
            GuildId = _guildId;

            ReactMessageId = 0;
        }
    }
}
