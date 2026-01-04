using CounterStrikeSharp.API.Core;

namespace ShowImages.Utils;

public static class PlayerExtensions
{
    public static bool IsTerrorist(this CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.PlayerPawn.IsValid && player.PlayerPawn.Value?.TeamNum == 2;
    }

    public static bool IsCounterTerrorist(this CCSPlayerController? player)
    {
        return player != null && player.IsValid && player.PlayerPawn.IsValid && player.PlayerPawn.Value?.TeamNum == 3;
    }
}