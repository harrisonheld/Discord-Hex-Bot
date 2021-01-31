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
            Console.WriteLine("Pingerz");
            if (!player.turn)
            {
                Program.BroadcastToUser(player.Info, "You have already moved!");
            }
            else
            {
                switch (command[0])
                {
                    case "move":
                        switch (command[1])
                        {
                            case "up":
                                player.Move(Direction.North);
                                player.turn = true;
                                break;
                            case "down":
                                player.Move(Direction.South);
                                player.turn = true;
                                break;
                            case "left":
                                player.Move(Direction.West);
                                player.turn = true;
                                break;
                            case "right":
                                player.Move(Direction.East);
                                player.turn = true;
                                break;
                            default:
                                Program.BroadcastToUser(player.Info, "Invalid Command");
                                break;
                        }
                        break;
                    case "shoot":
                        switch (command[1])
                        {
                            case "up":
                                player.Shoot(Direction.North);
                                player.turn = true;
                                break;
                            case "down":
                                player.Shoot(Direction.South);
                                player.turn = true;
                                break;
                            case "left":
                                player.Shoot(Direction.West);
                                player.turn = true;
                                break;
                            case "right":
                                player.Shoot(Direction.East);
                                player.turn = true;
                                break;
                            default:
                                Program.BroadcastToUser(player.Info, "Invalid Command");
                                break;
                        }
                        break;
                }
            }
            player.board.game.Step();
        }
    }
}
