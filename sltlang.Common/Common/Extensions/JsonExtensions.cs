using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Web;

namespace sltlang.Common.Common.Extensions
{
    public static class JsonExtensions
    {
        public static string SerializeByEnum<TEnum>(this object value, TEnum componentType) where TEnum : Enum
        {
            return JsonSerializer.Serialize(value, componentType.GetEnumComponentType());
        }

        public static object? DeserializeByEnum<TEnum>(this string value, TEnum componentType) where TEnum : Enum
        {
            return JsonSerializer.Deserialize(value, componentType.GetEnumComponentType());
        }

        public static T? Cast<T>(this object value) => value != null ? (T)value : default;

        public static string AddQueryParameter(this string uri, string key, object value)
        {
            return $"{uri}{(uri.Contains("?") ? "&" : "?")}{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value.ToString())}";
        }
    }
}
