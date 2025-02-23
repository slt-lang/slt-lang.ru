using sltlang.Domain.Models;
using sltlang.Domain.Ports;

namespace sltlang.Domain.Logic
{
    public class SampleStorage(ISampleMaker sampleMaker) : ISampleStorage
    {
        private readonly CodeSample[] codeSamples = sampleMaker.CodeFactory.Select(sampleMaker.CreateSample).ToArray();
        public IEnumerable<CodeSample> GetSamples() => codeSamples;
    }
}
