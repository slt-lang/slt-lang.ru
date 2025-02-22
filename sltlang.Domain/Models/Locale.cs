using System.Collections.Concurrent;

namespace sltlang.Domain.Models
{
    public class Locale
    {
        public readonly string Identifier;
        public static Locale Default;
        public readonly ConcurrentDictionary<string, string> Strings = new();

        public string this[string str]
        {
            get => Strings.TryGetValue(str, out var value) ? value : this != Default ? Default[str] : str;
        }

        public Locale(string identifier)
        {
            Identifier = identifier;
        }

        static Locale()
        {
        }
    }
}
