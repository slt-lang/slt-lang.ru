using sltlang.Common.AuthService.Models;

namespace sltlang.Domain
{
    public class Config
    {
        public IDictionary<string, string> PeerServices { get; set; } = default!;
        public JwtSettings JwtSettings { get; set; } = default!;
        public bool UseSecureCookie { get; set; }
    }
}
