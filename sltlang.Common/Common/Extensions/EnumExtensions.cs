using sltlang.Common.AuthService.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace sltlang.Common.Common.Extensions
{
    public static class EnumExtensions
    {
        public static class EnumHelper<T> where T: Enum
        {
            public static readonly T[] Values = (T[])Enum.GetValues(typeof(T));

            public static class ComponentType
            {
                private static ConcurrentDictionary<T, Type> variableTypes = new ConcurrentDictionary<T, Type>(typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public).Select(x => new KeyValuePair<T, Type>((T)x.GetRawConstantValue()!, x.GetCustomAttribute<ComponentTypeAttribute>()!.Type)));
                public static Type GetVariableType(T variable)
                {
                    return variableTypes[variable];
                }
            }

            public static class AttributeHelper<TAttribute> where TAttribute: Attribute
            {
                private static ConcurrentDictionary<T, TAttribute?> dict = new ConcurrentDictionary<T, TAttribute?>(typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public).Select(x => new KeyValuePair<T, TAttribute?>((T)x.GetRawConstantValue()!, x.GetCustomAttribute<TAttribute>())));

                public static IEnumerable<KeyValuePair<T, TAttribute?>> Map => dict;

                public static TValue? With<TValue>(T type, Func<TAttribute?, TValue?> func)
                {
                    return func(dict[type]);
                }

                public static bool HasAttribute(T type)
                {
                    return dict.Keys.Contains(type);
                }
            }
        }

        public static Dictionary<SecurityLevel, HashSet<Variable>> GetSecurityLevelVariables()
        {
            return EnumHelper<Variable>.AttributeHelper<SecurityLevelAttribute>.Map.Where(x => x.Value != null)
                .GroupBy(x => x.Value!.Level).ToDictionary(x => x.Key!, x => x.Where(x => x.Value != null).Select(x => x.Key).ToHashSet());
        }

        public static Type GetEnumComponentType<TEnum>(this TEnum commonType) where TEnum: Enum
        {
            return EnumHelper<TEnum>.ComponentType.GetVariableType(commonType);
        }

        public static bool HasAttribute<TEnum, TAttribute>(this TEnum variable) where TAttribute : Attribute where TEnum : Enum
        {
            return EnumHelper<TEnum>.AttributeHelper<TAttribute>.HasAttribute(variable);
        }

        public static UniversalResult ToUniversalResult<TEnum>(this TEnum value) where TEnum : Enum
            => new AnyEnumResult<TEnum>(value);
    }
}
