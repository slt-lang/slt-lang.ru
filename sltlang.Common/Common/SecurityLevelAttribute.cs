using System;

namespace sltlang.Common.Common
{
    public class SecurityLevelAttribute : Attribute
    {
        public SecurityLevel Level { get; set; }
        public SecurityLevelAttribute(SecurityLevel level) => Level = level;
    }
}
