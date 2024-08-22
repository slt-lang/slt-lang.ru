using System.Collections.Concurrent;
using System.Reflection;

namespace Specification
{
    public class LocaleService : ILocaleService
    {
        public ConcurrentDictionary<string, Locale> Locales { get; }
        private const string prefix = "Specification.locales.";
        public LocaleService()
        {
            var ass = Assembly.GetExecutingAssembly();
            var locales_res = ass.GetManifestResourceNames().Where(x => x.StartsWith(prefix)).Select(x => (x, SLThree.Extensions.SLThreeExtensions.ReadStrings(ass.GetManifestResourceStream(x)))).Select(x => (x.x.Substring(prefix.Length), x.Item2));

            Locales = new ConcurrentDictionary<string, Locale>(locales_res.Select(x => x.Item1.Substring(0, x.Item1.IndexOf('.'))).Distinct().Select(x => new KeyValuePair<string, Locale>(x, new Locale(x))));
            foreach (var x in locales_res)
            {
                var loc = Locales[x.Item1.Substring(0, x.Item1.IndexOf('.'))].Strings;
                foreach (var str in x.Item2)
                {
                    if (string.IsNullOrWhiteSpace(str) || str.StartsWith("//")) continue;
                    var ind = str.IndexOf('=');
                    var key = str.Substring(0, ind);
                    var value = str.Substring(ind + 1, str.Length - ind - 1);
                    while (!loc.TryAdd(key, value)) ;
                }
            }
            Locale.Default = Locales.TryGetValue("en", out var en) ? en : Locales.First().Value;
        }
    }
}
