using Microsoft.AspNetCore.Http;
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
        private static bool HasVariable<T>(this ClaimsPrincipal user, Variable variable, out T? value)
        {
            var claim = user!.Claims.FirstOrDefault(x => x.Type == "Variable+" + variable.ToString());
            if (claim != null)
            {
                value = (T)claim.Value.DeserializeByEnum(variable)!;
                return true;
            }
            value = default;
            return false;
        }

        private static T? GetDefaultVariable<T>(Variable variable, bool even_if_unauthorized)
        {
            if (!even_if_unauthorized)
                return default!;

            var defaultvalue = EnumExtensions.EnumHelper<Variable>.AttributeHelper<DefaultValueAttribute>.With(variable, attr => attr?.Value);
            if (defaultvalue != null)
            {
                return defaultvalue is string ? (T)$"\"{defaultvalue}\"".ToString()?.DeserializeByEnum(variable)! : (T)defaultvalue!;
            }

            return default!;
        }

        public static T? GetVariable<T>(this ClaimsPrincipal user, Variable variable, bool even_if_unauthorized = false)
        {
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                if (HasVariable<T>(user, variable, out var claim))
                    return claim;
            }

            return GetDefaultVariable<T>(variable, even_if_unauthorized);
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

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!);
        }
    }
}
