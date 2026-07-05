using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;

using ShowImages.Utils;
using ShowImages.Configs;
using ShowImages.Commands;

namespace ShowImages;

[MinimumApiVersion(369)]
public class ShowImages : BasePlugin, IPluginConfig<BaseConfig>
{
    public override string ModuleName => "ShowImages";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "luca.uy";
    public override string ModuleDescription => "Show images";

    public required BaseConfig Config { get; set; }

    private PermissionValidator? _permissionValidator;
    private Dictionary<string, ShowImageCommand> _registeredCommands = new();

    public override void Load(bool hotReload)
    {
        Utils.Logger.Config = Config;
        _permissionValidator = new PermissionValidator(Localizer);

        InitializeCommands();

        Utils.Logger.LogInfo("Core", "Plugin loaded successfully");
    }

    public void OnConfigParsed(BaseConfig config)
    {
        Config = config;
        Utils.Logger.Config = config;
        Utils.Logger.LogInfo("Config", "Configuration loaded successfully");

        if (_permissionValidator != null)
        {
            RegisterImageCommands();
        }
    }

    private void InitializeCommands()
    {
        RegisterImageCommands();
        Utils.Logger.LogDebug("Core", "Commands registered");
    }

    private void RegisterImageCommands()
    {
        if (Config?.ImageCommands == null)
        {
            return;
        }

        foreach (var kvp in Config.ImageCommands)
        {
            string commandName = kvp.Key;
            string permissionFlag = kvp.Value?.PermissionFlag ?? "";

            if (_registeredCommands.ContainsKey(commandName))
            {
                continue;
            }

            var command = new ShowImageCommand(
                commandName,
                permissionFlag,
                _permissionValidator!,
                Localizer
            );

            _registeredCommands[commandName] = command;
            AddCommand($"css_{commandName}", "Show image", command.Execute);

            Utils.Logger.LogDebug("Core", $"Command {commandName} registered with permission flag: {(string.IsNullOrWhiteSpace(permissionFlag) ? "none (all)" : permissionFlag)}");
        }
    }

    public override void Unload(bool hotReload)
    {
        Utils.Logger.LogInfo("Core", "Plugin unloaded successfully");
    }
}