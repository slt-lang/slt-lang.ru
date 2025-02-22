using SLThree.Extensions;
using sltlang.Domain.Models;
using sltlang.Domain.Ports;
using System.Reflection;

namespace sltlang.Adapters.Adapters
{
    public class SyntaxPageMaker(ISampleLogic sampleLogic, ILocaleService localeService) : ISyntaxPageMaker
    {
        public IEnumerable<Type> PageFactory => SampleMaker.ListedTypes;

        private class SyntaxPagePreCache
        {
            public Type[] Hierarchy { get; set; } = default!;
            public Type[] Inheritors { get; set; } = default!;
            public Type[] Interfaces { get; set; } = default!;
        }
        private Dictionary<SyntaxPage, SyntaxPagePreCache> precache = new Dictionary<SyntaxPage, SyntaxPagePreCache>();
        public SyntaxPage CreateSyntaxPage(Type type, string locale)
        {
            var cache = new SyntaxPagePreCache()
            {
                Hierarchy = GetAncestors(type).Reverse().ToArray(),
                Inheritors = type.Assembly.GetTypes().Where(x => x.BaseType == type && (x.IsPublic || x.IsNestedPublic)).ToArray(),
                Interfaces = type.GetInterfaces()
            };

            var name = type.Name;

            var ret = new SyntaxPage()
            {
                CultureKey = locale,
                Name = name,
                Link = $"/{locale}/syntax/{name}",
                Type = type,
            };
            precache.Add(ret, cache);

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var memberlist = new List<SyntaxPage.Member>();
            foreach (var field in fields)
            {
                memberlist.Add(new SyntaxPage.Member()
                {
                    IsReadonly = field.IsInitOnly,
                    IsStatic = field.IsStatic,
                    Value = null!,
                    Name = field.Name,
                    Type = field.FieldType.MakeNode(locale),
                });
            }
            foreach (var property in properties)
            {
                memberlist.Add(new SyntaxPage.Member()
                {
                    IsReadonly = !property.CanWrite,
                    IsStatic = false,
                    Value = null!,
                    Name = property.Name,
                    Type = property.PropertyType.MakeNode(locale),
                });
            }

            ret.Members = memberlist.ToArray();

            ret.CodeSamples = sampleLogic.GetSamples(type, locale);

            return ret;
        }
        public static IEnumerable<Type> GetAncestors(Type type)
        {
            do
            {
                yield return type;
                type = type.BaseType!;
            }
            while (type != null);
        }

        public SyntaxPage[] Linking(SyntaxPage[] syntaxPages)
        {
            foreach (var sp in syntaxPages)
            {
                if (precache.TryGetValue(sp, out var cache))
                {
                    sp.Inheritors = cache.Inheritors.Select(x => syntaxPages.First(y => y.Type == x && y.CultureKey == sp.CultureKey)).ToArray();
                    sp.Hierarchy = cache.Hierarchy.Select(x => SampleMaker.ListedTypes.Contains(x) ? syntaxPages.First(y => y.Type == x && y.CultureKey == sp.CultureKey) : x.MakeNode(sp.CultureKey)).ToArray();
                    sp.Interfaces = cache.Interfaces.Select(x => SampleMaker.ListedTypes.Contains(x) ? syntaxPages.First(y => y.Type == x && y.CultureKey == sp.CultureKey) : x.MakeNode(sp.CultureKey)).ToArray();
                }
            }
            return syntaxPages;
        }
    }
}
