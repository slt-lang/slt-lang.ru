using sltlang.Common.AuthService.Enums;
using sltlang.Common.Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Adapters.Extensions
{
    public static class AuthExtensions
    {
        public static T? GetVariable<T>(this ClaimsPrincipal user, Variable variable)
        {
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                var value = user!.Claims.FirstOrDefault(x => x.Type == "Variable+" + variable.ToString());
                if (value != null)
                {
                    return (T)value.Value.DeserializeByEnum(variable)!;
                }
            }

            var defaultvalue = EnumExtensions.EnumHelper<Variable>.AttributeHelper<DefaultValueAttribute>.With(variable, attr => attr?.Value);
            if (defaultvalue != null)
            {
                return defaultvalue is string ? (T)$"\"{defaultvalue}\"".ToString()?.DeserializeByEnum(variable)! : (T)defaultvalue!;
            }

            return default!;
        }

        public static bool HasPermission(this ClaimsPrincipal user, Permission permission)
        {
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                return user!.Claims.Any(x => x.Type == "Permission" && x.Value == permission.ToString());
            }
            return false;
        }

        public static IEnumerable<Permission> GetPermissions(this ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                return user!.Claims.Where(x => x.Type == "Permission").Select(x => Enum.Parse<Permission>(x.Value));
            }
            return [];
        }
    }
}
