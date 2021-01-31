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
                                player.Move(EmojiWord.North);
                                break;
                            case "down":
                                player.Move(EmojiWord.South);
                                break;
                            case "left":
                                player.Move(EmojiWord.West);
                                break;
                            case "right":
                                player.Move(EmojiWord.East);
                                break;
                            default:
                                Program.BroadcastToUser(player.Info, "Invalid Command!");
                                return;
                        }
                        break;
                    case "shoot":
                        switch (command[1])
                        {
                            case "up":
                                player.Shoot(EmojiWord.North);
                                break;
                            case "down":
                                player.Shoot(EmojiWord.South);
                                break;
                            case "left":
                                player.Shoot(EmojiWord.West);
                                break;
                            case "right":
                                player.Shoot(EmojiWord.East);
                                break;
                            default:
                                Program.BroadcastToUser(player.Info, "Invalid Command!");
                                return;
                        }
                        break;
                }
                player.game.Step();
            }
        }
    }
}
