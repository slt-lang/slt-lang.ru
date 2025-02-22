using SLThree.Language;

namespace sltlang.Adapters.Adapters
{
    public class SLThreeHtmlRestorator : Restorator
    {
        public SLThreeHtmlRestorator() : base()
        {
            Writer = new HtmlWriter();
        }

        public class HtmlWriter : DefaultRestoratorWriter
        {
            private static string OpenTag(string tag, string @class) => $"<{tag}{(@class != null ? $" class=\"{@class}\"" : "")}>";
            private static string CloseTag(string tag) => $"</{tag}>";
            private static string Html(string str, string tag, string @class)
            {
                return $"{OpenTag(tag, @class)}{str.Replace("<", "&lt;").Replace(">", "&gt;")}{CloseTag(tag)}";
            }

            public override void WriteCallText(string s)
            {
                Sb.Append(Html(s, "span", "slt-call"));
            }
            public override void WriteExpressionKeyword(string s)
            {
                Sb.Append(Html(s, "span", "slt-keyword1"));
            }
            public override void WriteStatementKeyword(string s)
            {
                Sb.Append(Html(s, "span", "slt-keyword2"));
            }
            public override void WriteTab()
            {
                Sb.Append(new string(' ', Tabulation * Level).Replace(" ", "&nbsp;"));
            }
            public override void WriteTypeText(string s)
            {
                Sb.Append(Html(s, "span", "slt-type"));
            }
            public override void WritePlainText(string s)
            {
                Sb.Append(Html(s, "span", "slt-plain"));
            }
            public override void WritelnPlainText(string s)
            {
                Sb.AppendLine(Html(s, "span", "slt-plain"));
            }

            public override void WriteStringText(string s)
            {
                Sb.Append(Html(s, "span", "slt-string"));
            }

            public override void WriteErrorText(string s)
            {
                Sb.Append(Html(s, "span", "slt-error"));
            }

            public override void WriteDigitText(string s)
            {
                Sb.Append(Html(s, "span", "slt-digit"));
            }
        }
    }
}
