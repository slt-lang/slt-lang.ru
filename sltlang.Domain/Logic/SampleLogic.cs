using sltlang.Domain.Models;
using sltlang.Domain.Ports;

namespace sltlang.Domain.Logic
{
    public class SampleLogic(ISampleStorage sampleStorage) : ISampleLogic
    {
        public IEnumerable<CodeSample> GetSamples(Type type, string culture, int count = 3)
        {
            return sampleStorage
                .GetSamples()
                .Where(x => (x.CultureKey == MultiCultureKey.Value || x.CultureKey == culture) && x.CodeStatistic[type] > 0)
                .OrderByDescending(x => x.CodeStatistic[type])
                .Take(count);
        }
        public IEnumerable<CodeSample> GetSamples<T>(string culture, int count = 3) => GetSamples(typeof(T), culture, count);
    }
}
