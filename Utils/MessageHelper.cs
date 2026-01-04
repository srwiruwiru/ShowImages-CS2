using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ShowImages.Utils;

public static class MessageHelper
{
    public static void SendMessage(CCSPlayerController? caller, string message)
    {
        if (caller != null && caller.IsValid)
        {
            caller.PrintToChat(message);
        }
        else
        {
            Server.PrintToChatAll(message);
        }
    }
}