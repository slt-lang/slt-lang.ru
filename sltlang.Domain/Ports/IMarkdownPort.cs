using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sltlang.Domain.Models;

namespace sltlang.Domain.Ports
{
    public interface IMarkdownPort
    {
        string ToHtml(string text, MarkdownContext markdownContext);
    }
}
