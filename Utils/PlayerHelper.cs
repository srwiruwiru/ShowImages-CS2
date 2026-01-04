using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;

namespace ShowImages.Utils;

public static class PlayerHelper
{
    public static IEnumerable<CCSPlayerController> GetValidPlayers()
    {
        return Utilities.GetPlayers().Where(p => p.IsValid && !p.IsBot && !p.IsHLTV);
    }

    public static IEnumerable<CCSPlayerController> GetPlayersByVisibility(string visibility)
    {
        var players = GetValidPlayers();

        return visibility.ToLower() switch
        {
            "@ct" => players.Where(p => p.IsCounterTerrorist()),
            "@t" => players.Where(p => p.IsTerrorist()),
            "@all" or _ => players
        };
    }

    public static void PrintToCenterHtmlForPlayers(IEnumerable<CCSPlayerController> players, string htmlContent, int duration = 5)
    {
        foreach (var player in players)
        {
            player.PrintToCenterHtml(htmlContent, duration);
        }
    }

    public static void ShowImage(string visibility, string imageUrl, int durationSeconds)
    {
        var htmlContent = $"<center><img src='{imageUrl}' width='640' height='360'></center>";
        var players = GetPlayersByVisibility(visibility);
        PrintToCenterHtmlForPlayers(players, htmlContent, durationSeconds);
    }
}