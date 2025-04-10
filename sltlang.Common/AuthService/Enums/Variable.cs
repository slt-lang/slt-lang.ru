using sltlang.Common.Common;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace sltlang.Common.AuthService.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Variable
    {
        [Description("Отображаемое имя")]
        [ComponentType(typeof(string))]
        [SecurityLevel(SecurityLevel.Low)]
        [DefaultValue("Новый участник")]
        [AlwaysTranfsfer]
        DisplayName,
        [Description("TTL ссылки-приглашения (минуты)")]
        [ComponentType(typeof(int))]
        [SecurityLevel(SecurityLevel.High)]
        [DefaultValue(24 * 60)]
        MaxLinkTTL,
    }
}
