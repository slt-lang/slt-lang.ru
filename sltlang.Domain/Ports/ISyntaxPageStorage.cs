using sltlang.Domain.Models;

namespace sltlang.Domain.Ports
{
    public interface ISyntaxPageStorage
    {
        IEnumerable<SyntaxPage> GetPages();
    }
}
