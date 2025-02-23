using sltlang.Domain.Models;

namespace sltlang.Domain.Ports
{
    public interface ISampleMaker
    {
        IEnumerable<(string FileName, string Code)> CodeFactory { get; }
        CodeSample CreateSample((string FileName, string Code) res);
    }
}
