using sltlang.Domain.Models;

namespace sltlang.Domain.Ports
{
    public interface ISyntaxPageMaker
    {
        IEnumerable<Type> PageFactory { get; }
        SyntaxPage CreateSyntaxPage(Type type, string locale);
        SyntaxPage[] Linking(SyntaxPage[] syntaxPages);
    }
}
