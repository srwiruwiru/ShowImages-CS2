using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace ShowImages.Configs;

public class BaseConfig : BasePluginConfig
{
    [JsonPropertyName("ImageCommands")]
    public Dictionary<string, ImageCommandConfig> ImageCommands { get; set; } = new()
    {
        {
            "showimage",
            new ImageCommandConfig
            {
                PermissionFlag = "@css/root"
            }
        }
    };

    [JsonPropertyName("EnableDebug")]
    public bool EnableDebug { get; set; } = false;
}

public class ImageCommandConfig
{
    [JsonPropertyName("PermissionFlag")]
    public string PermissionFlag { get; set; } = "";
}