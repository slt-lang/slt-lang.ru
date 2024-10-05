using SLThree.Extensions;
using SLThree.Language;
using Specification;

namespace sltlang
{
    public static class SpecHelper
    {
        public static IEnumerable<Type> GetAncestors(Type type)
        {
            do
            {
                yield return type;
                type = type.BaseType;
            }
            while (type != null);
        }
        public static string ReplaceCompares(string str)
        {
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace(".", "<wbr>.");
            str = str.Replace("+", "<wbr>.");
            return str;
        }
        public static string MakeGenericLink(Type type, Locale locale)
        {
            if (type.IsArray)
            {
                if (type.GetArrayRank() != 1)
                {
                    return $"array{type.GetArrayRank()}&lt;{MakeLink(type.GetElementType(), locale)}&gt;";
                }

                return "array&lt;" + MakeLink(type.GetElementType(), locale) + "&gt;";
            }

            if (type.IsGenericTypeDefinition)
            {
                int num = type.GetGenericArguments().Length;
                return type.FullName.Substring(0, type.FullName.IndexOf('`')).Split('.').Last() + "&lt;" + ((num > 1) ? new string(',', num) : "") + "&gt;";
            }

            Type genericTypeDefinition = type.GetGenericTypeDefinition();
            string empty = string.Empty;
            empty = ((genericTypeDefinition == typeof(List<>)) ? "list" : ((genericTypeDefinition == typeof(Dictionary<,>)) ? "dict" : ((genericTypeDefinition == typeof(Stack<>)) ? "stack" : ((genericTypeDefinition == typeof(Queue<>)) ? "queue" : (type.Name.StartsWith("ValueTuple") ? "tuple" : ((type.FullName == null) ? type.Name.Substring(0, type.Name.IndexOf('`')).Split('.').Last() : type.FullName.Substring(0, type.FullName.IndexOf('`')).Split('.').Last()))))));
            return empty + "&lt;" + type.GetGenericArguments().ConvertAll((Type x) => MakeLink(x, locale)).JoinIntoString(", ") + "&gt;";
        }
        public static string MakeLink(Type type, Locale locale)
        {
            if (type.IsGenericType || type.IsArray || type.IsGenericTypeDefinition)
                return $"<span class=\"slt-type\">{MakeGenericLink(type, locale)}</span>";
            if (sltlang.Controllers.SpecificationController.TypesWithArticle.Values.Contains(type))
                return $"<a href=\"/{locale.Identifier}/specification/{sltlang.Controllers.SpecificationController.TypesWithArticle.First(x => x.Value == type).Key}\">{ReplaceCompares(type.GetTypeString())}</a>";
            if (sltlang.Controllers.SpecificationController.SupportedTypes.Contains(type))
                return $"<a href=\"/{locale.Identifier}/specification/{type.Name.ToLower()}\">{ReplaceCompares(type.GetTypeString())}</a>";
            return $"<span class=\"slt-type\">{ReplaceCompares(type.GetTypeString())}</span>";
        }

        private static readonly SLThreeHtml Restorator = new();
        private static readonly Parser Parser = new();
        public static string SLThreeCode(string code, Dictionary<string, object> options)
        {
            var lines = Restorator.Restore(Parser.ParseScript(code.Trim()), new SLThree.ExecutionContext(false, false, SLThree.LocalVariablesContainer.GetFromDictionary(options))).Split("\n").ToList();
            if (string.IsNullOrWhiteSpace(lines[lines.Count - 1])) lines.RemoveAt(lines.Count - 1);
            var hlines = lines.Select(x =>
            {
                return $"<li>{x}</li>";
            });
            return "<div class=\"textbox slt-code code\"><ol>" + hlines.JoinIntoString("") + "</ol></div>";
        }
    }
}
