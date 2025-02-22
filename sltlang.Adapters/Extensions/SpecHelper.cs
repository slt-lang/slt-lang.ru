using SLThree.Extensions;
using SLThree.Language;
using sltlang.Adapters.Adapters;
using sltlang.Domain.Models;

namespace sltlang
{
    public static class SpecHelper
    {
        public static SyntaxNode MakeNode(this Type type, string culturekey)
        {
            var name = type.Name;
            return new SyntaxNode()
            {
                Type = type,
                Class = "slt-type",
                Link = SampleMaker.ListedTypes.Contains(type) ? $"/{culturekey}/syntax/{name}" : null!,
                Name = name.ToLower(),
            };
        }
    }
}
