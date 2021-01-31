using System;
using System.Collections.Generic;
using System.Text;
using Discord_Hex_Bot.game.math;

namespace Discord_Hex_Bot.game.io
{
    public abstract class HandleInput
    {
        public static void handleInput(UserInfo info, String[] command)
        {
            handleInput(GameInstance.INSTANCE.getPlayerFromInfo(info), command);
        }
        public static void handleInput(entity.Player player, String[] command)
        {
            GameInstance gameInstance = GameInstance.INSTANCE;
            if (player.turn)
            {
                Program.BroadcastToUser(player.Info, "You have already moved!");
            }
            switch(command[0])
            {
                case "move":
                    switch (command[1])
                    {
                        case "up":
                            player.Move(Direction.North);
                            break;
                        case "down":
                            player.Move(Direction.South);
                            break;
                        case "left":
                            player.Move(Direction.West);
                            break;
                        case "right":
                            player.Move(Direction.East);
                            break;
                        default:
                            Program.BroadcastToUser(userInfo, "Invalid Command");
                            break;
                    }
                    break;
                case "shoot":
                    switch (command[1])
                    {
                        case "up":
                            player.Shoot(Direction.North);
                            break;
                        case "down":
                            player.Shoot(Direction.South);
                            break;
                        case "left":
                            player.Shoot(Direction.West);
                            break;
                        case "right":
                            player.Shoot(Direction.East);
                            break;
                    }
                    break;
            }
        }
    }
}
