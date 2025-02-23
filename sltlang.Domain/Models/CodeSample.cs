using System.Collections.Concurrent;

namespace sltlang.Domain.Models
{
    public class CodeSample
    {
        public string CultureKey { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Dictionary<object, object> LineComments { get; set; } = default!;
        public ConcurrentDictionary<Type, double> CodeStatistic { get; set; } = default!;
        public string ReadyHtml { get; set; } = default!;
    }
}
