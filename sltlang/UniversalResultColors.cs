using sltlang.Common.AuthService.Contracts;
using sltlang.Common.Common;
using System.Collections.Concurrent;

namespace sltlang
{
    public static class UniversalResultColors
    {
        public const string Alert = "var(--notifics-alert)";
        public const string Warn = "var(--notifics-warn)";
        public const string Success = "var(--notifics-success)";
        public const string Informational = "var(--notifics-info)";
        public static readonly ConcurrentDictionary<string, string> EnumColors = new ConcurrentDictionary<string, string>(
            new Dictionary<object, string>()
            {
                { CreateInviteResult.PermissionsError, Warn },
                { CreateInviteResult.Success, Success },
                { CreateInviteResult.TooManyInvites, Warn },
                { CreateInviteResult.UnknownError, Alert },
                { CreateInviteResult.InviteAlreadyExists, Informational },
            }
            .Select(x => new KeyValuePair<string, string>($"UR-{x.Key.GetType().Name}-{x.Key}", x.Value))
        );

        public static string GetStyle(this string result)
        {
            if (EnumColors.TryGetValue(result, out var color)) 
                return $"background-color: {color};";
            return "";
        }

        public static string GetStyle(this UniversalResult universalResult) => GetStyle(universalResult.ToString());
    }
}
