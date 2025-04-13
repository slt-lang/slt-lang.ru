using sltlang.Common.AuthService.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Common.Common
{
    public class UniversalResult
    {
        public virtual string Result { get; set; } = default!;
        public override string ToString() => $"UR-{Result}";
    }

    public class AnyEnumResult<T>(T value) : UniversalResult where T: Enum
    {
        public static readonly string TName = typeof(T).Name;
        public AnyEnumResult() : this(default!) { }
        public T Value { get; set; } = value;
        public override string Result => $"{TName}-{Value}";
    }
}
