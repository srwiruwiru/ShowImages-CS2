using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Localization;
using System.Linq;

using ShowImages.Utils;

namespace ShowImages.Commands;

public class ShowImageCommand
{
    private readonly PermissionValidator _permissionValidator;
    private readonly IStringLocalizer _localizer;
    private readonly string _permissionFlag;
    private readonly string _commandName;

    public ShowImageCommand(string commandName, string permissionFlag, PermissionValidator permissionValidator, IStringLocalizer localizer)
    {
        _commandName = commandName;
        _permissionFlag = permissionFlag;
        _permissionValidator = permissionValidator;
        _localizer = localizer;
    }

    public void Execute(CCSPlayerController? caller, CommandInfo info)
    {
        if (!_permissionValidator.ValidatePermission(caller, _permissionFlag))
        {
            return;
        }

        Logger.LogDebug("ShowImageCommand", $"ArgCount: {info.ArgCount}");
        for (int i = 0; i < info.ArgCount; i++)
        {
            Logger.LogDebug("ShowImageCommand", $"Arg({i}): '{info.GetArg(i)}'");
        }

        if (info.ArgCount < 4)
        {
            var message = $"{_localizer["common.prefix"]} {_localizer["error.invalidArguments"]}";
            MessageHelper.SendMessage(caller, message);
            Logger.LogWarning("ShowImageCommand", $"Invalid arguments for command {_commandName}. ArgCount: {info.ArgCount}");
            return;
        }

        string visibility = info.GetArg(1).ToLower();
        if (visibility != "@ct" && visibility != "@t" && visibility != "@all")
        {
            var message = $"{_localizer["common.prefix"]} {_localizer["error.invalidVisibility"]}";
            MessageHelper.SendMessage(caller, message);
            Logger.LogWarning("ShowImageCommand", $"Invalid visibility: {visibility}");
            return;
        }

        string imageUrlArg = info.GetArg(2);
        string imageUrl = imageUrlArg.Trim('"', '\'');

        string durationArg = info.GetArg(3);
        Logger.LogDebug("ShowImageCommand", $"Duration argument: '{durationArg}'");
        
        if (string.IsNullOrWhiteSpace(durationArg))
        {
            string[] urlParts = imageUrlArg.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (urlParts.Length > 1)
            {
                string lastPart = urlParts[urlParts.Length - 1].Trim('"', '\'');
                if (int.TryParse(lastPart, out int parsedDuration))
                {
                    durationArg = lastPart;
                    imageUrl = string.Join(" ", urlParts.Take(urlParts.Length - 1)).Trim('"', '\'');
                    Logger.LogDebug("ShowImageCommand", $"Extracted duration from URL argument: '{durationArg}', URL: '{imageUrl}'");
                }
            }
        }
        
        if (string.IsNullOrWhiteSpace(durationArg) || !int.TryParse(durationArg, out int durationSeconds) || durationSeconds <= 0)
        {
            var message = $"{_localizer["common.prefix"]} {_localizer["error.invalidDuration"]}";
            MessageHelper.SendMessage(caller, message);
            Logger.LogWarning("ShowImageCommand", $"Invalid duration: '{durationArg}' (ArgCount: {info.ArgCount})");
            return;
        }

        PlayerHelper.ShowImage(visibility, imageUrl, durationSeconds);

        Logger.LogInfo("ShowImageCommand", $"Image displayed via {_commandName} by {caller?.PlayerName ?? "Console"}: {imageUrl} ({durationSeconds}s)");
    }
}