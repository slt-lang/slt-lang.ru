using Microsoft.Extensions.Diagnostics.HealthChecks;
using SLThree;
using SLThree.Language;
using SLThree.Metadata;

namespace sltlang
{
    public class Pizdec<T> : T where T: DefaultRestorator
    {

    }

    public class SLThreeHtml : Restorator
    {
        private static string OpenTag(string tag, string @class) => $"<{tag}{(@class != null ? $" class=\"{@class}\"" : "")}>";
        private static string CloseTag(string tag) => $"</{tag}>";
        private static string Html(string str, string tag, string @class)
        {
            return $"{OpenTag(tag, @class)}{str.Replace("<", "&lt;").Replace(">", "&gt;")}{CloseTag(tag)}";
        }

        public override void WriteCallText(string s)
        {
            sb.Append(Html(s, "span", "slt-call"));
        }
        public override void WriteExpressionKeyword(string s)
        {
            sb.Append(Html(s, "span", "slt-keyword1"));
        }
        public override void WriteStatementKeyword(string s)
        {
            sb.Append(Html(s, "span", "slt-keyword2"));
        }
        public override void WriteTab()
        {
            sb.Append(new string(' ', Tabulation * Level).Replace(" ", "&nbsp;"));
        }
        public override void WriteTypeText(string s)
        {
            sb.Append(Html(s, "span", "slt-type"));
        }
        public override void WritePlainText(string s)
        {
            sb.Append(Html(s, "span", "slt-plain"));
        }
        public override void WritelnPlainText(string s)
        {
            sb.AppendLine(Html(s, "span", "slt-plain"));
        }
    }
}
