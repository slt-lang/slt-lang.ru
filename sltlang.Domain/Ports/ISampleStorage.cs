using sltlang.Domain.Models;

namespace sltlang.Domain.Ports
{
    public interface ISampleStorage
    {
        IEnumerable<CodeSample> GetSamples();
    }
}
