using System.Collections.Concurrent;

namespace sltlang.Domain
{
    public class Config
    {
        public IDictionary<string, string> PeerServices { get; set; } = default!;
    }
}
