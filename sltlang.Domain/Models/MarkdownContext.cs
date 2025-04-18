using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sltlang.Domain.Models
{
    public class MarkdownContext
    {
        public string CultureKey { get; set; } = default!;
        public Func<string, string> CodeHtmlFinalizer { get; set; } = x => x;
    }
}
