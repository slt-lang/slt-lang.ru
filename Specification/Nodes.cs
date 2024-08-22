using SLThree.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Specification
{
    public class Content(string @string, bool isHtml) : ISpecificationNode
    {
        public string String { get; set; } = @string;
        public virtual bool IsHtml { get; private set; } = isHtml;
        public virtual string ContentView(IHyperContext hyperContext)
        {
            var result = String;
            if (!IsHtml) result = HtmlReplace(result);
            return result;
        }
        public virtual string View(IHyperContext context) => ContentView(context);
        public static string HtmlReplace(string target)
        {
            var result = target;
            result = result.Replace("<", "&lt;");
            result = result.Replace(">", "&gt;");
            return result;
        }
    }

    public class Formattable : Content
    {
        public ISpecificationNode[] Arguments { get; set; }

        public Formattable(string formatString, params ISpecificationNode[] arguments) : base(formatString, true)
        {
            String = formatString;
            Arguments = arguments;
        }
        public Formattable(string formatString, bool isHtml = true, params ISpecificationNode[] arguments) : base(formatString, isHtml)
        {
            String = formatString;
            Arguments = arguments;
        }

        public override string ContentView(IHyperContext hyperContext)
        {
            var result = string.Format(String, Arguments.ConvertAll(x => x.View(hyperContext)));
            if (!IsHtml) result = HtmlReplace(result);
            return result;
        }
    }

    public class Tag(string tag, string formatString, params ISpecificationNode[] arguments) : Formattable(formatString, true, arguments)
    {
        public class Paragraph(string formatString, params ISpecificationNode[] arguments) : Tag("p", formatString, arguments) { }
        public class Bold(string formatString, params ISpecificationNode[] arguments) : Tag("strong", formatString, arguments) { }
        public class Cursive(string formatString, params ISpecificationNode[] arguments) : Tag("i", formatString, arguments) { }

        public string TagString { get; set; } = tag;

        public override string View(IHyperContext context)
        {
            return $"<{TagString}>{ContentView(context)}</{TagString}>";
        }
    }
}
