using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sltlang.Domain.Models;
using sltlang.Domain.Ports;
using Markdig;

namespace sltlang.Adapters.Adapters
{
    public class MarkdownPort : IMarkdownPort
    {
        public static string CodePostProcessing(string input, MarkdownContext markdownContext)
        {
            const string codeLeft = "<code class=\"language-SLThree\">";
            const string codeRight = "</code>";

            var sb = new StringBuilder();
            var startIndex = 0;
            while (true)
            {
                var next = input.IndexOf(codeLeft, startIndex);
                var nextEnd = input.IndexOf(codeRight, startIndex);
                if (next == -1 || nextEnd == -1)
                {
                    sb.Append(input.Substring(startIndex, input.Length - startIndex));
                    break;
                }

                var prefix = input.Substring(startIndex, next - startIndex);
                var code = markdownContext.CodeHtmlFinalizer(input.Substring(next + codeLeft.Length, nextEnd - (next + codeLeft.Length)).Replace("&gt;", ">").Replace("&lt;", "<"));
                startIndex = nextEnd + codeRight.Length;
                sb.Append($"{prefix}{code}");
            }

            return sb.ToString();
        }

        public string ToHtml(string text, MarkdownContext markdownContext)
        {
            var html = Markdown.ToHtml(text);

            html = html.Replace("$CULTURE_KEY$", markdownContext.CultureKey);
            html = CodePostProcessing(html, markdownContext);

            return html;
        }
    }
}
