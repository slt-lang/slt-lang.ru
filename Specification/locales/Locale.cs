using SLThree;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static SLThree.GenericMethod.GenericInfo;

namespace Specification
{

    public class Locale
    {
        public readonly string Identifier;
        internal static Locale Default;
        public readonly ConcurrentDictionary<string, string> Strings = new();

        public string this[string str]
        {
            get => Strings.TryGetValue(str, out var value) ? value : (this != Default) ? Default[str] : str;
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
