using sltlang.Domain.Models;
using System.Collections.Concurrent;

namespace sltlang.Domain.Ports
{
    public interface ILocaleService
    {
        public ConcurrentDictionary<string, Locale> Locales { get; }
    }
}
