using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using Microsoft.Extensions.Localization;

namespace ShowImages.Utils;

public class PermissionValidator
{
    private readonly IStringLocalizer _localizer;

    public PermissionValidator(IStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    public bool HasPermission(CCSPlayerController? caller, string permissionFlag)
    {
        if (caller == null) return true;
        if (string.IsNullOrWhiteSpace(permissionFlag)) return true;
        return AdminManager.PlayerHasPermissions(caller, permissionFlag);
    }

    public bool ValidatePermission(CCSPlayerController? caller, string permissionFlag)
    {
        if (caller != null && !HasPermission(caller, permissionFlag))
        {
            var message = $"{_localizer["common.prefix"]} {_localizer["error.noPermission"]}";
            MessageHelper.SendMessage(caller, message);
            return false;
        }
        return true;
    }
}