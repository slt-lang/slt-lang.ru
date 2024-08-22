using System.Collections.Concurrent;

namespace Specification
{
    public interface ILocaleService
    {
        public ConcurrentDictionary<string, Locale> Locales { get; }
    }
}
