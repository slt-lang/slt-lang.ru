using sltlang.Domain.Models;

namespace sltlang.Domain.Ports
{
    public interface ISampleLogic
    {
        IEnumerable<CodeSample> GetSamples(Type type, string culture, int count = 3);
        IEnumerable<CodeSample> GetSamples<T>(string culture, int count = 3);
    }
}
