using SLThree;
using SLThree.Language;

namespace sltlang
{
    public class SLThreeHtml : Restorator
    {
        private static string OpenTag(string tag, string @class) => $"<{tag}{(@class != null ? $" class=\"{@class}\"" : "")}>";
        private static string CloseTag(string tag) => $"</{tag}>";
        private static string Html(string str, string tag, string @class)
        {
            return $"{OpenTag(tag, @class)}{str}{CloseTag(tag)}";
        }

        public override void VisitExpression(Special expression)
        {
            sb.Append(Html(expression.ToString(), "span", "slt-keyword1"));
        }
        public override void VisitExpression(Literal expression)
        {
            sb.Append(Html(expression.RawRepresentation, "span", expression switch
            {
                StringLiteral => "slt-string",
                AtomLiteral => "slt-plain",
                _ => "slt-digit",
            }));
        }
    }
}
