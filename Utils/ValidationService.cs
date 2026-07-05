using CounterStrikeSharp.API.Core;

namespace ShowImages.Utils;

public static class ValidationService
{
    public static bool IsValidPlayer(CCSPlayerController? player)
    {
        return player != null && player.IsValid && !player.IsBot && !player.IsHLTV && player.Connected == PlayerConnectedState.Connected;
    }
}