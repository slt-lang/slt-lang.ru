using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace Specification
{
    public interface IHyperOptions
    {
        object this[string ident] { get; }
    }

    public interface IHyperContext
    {
        Locale Localization { get; }
        IHyperOptions Options { get; }
    }

    public interface ISpecificationNode
    {
        string View(IHyperContext context);
    }

    public interface IArticle
    {
        public string Key { get; }
        public IList<ISpecificationNode> Nodes { get; }
        public string Generate(IHyperContext context)
        {
            var sb = new StringBuilder();
            foreach (var node in Nodes)
                sb.Append(node.View(context));
            return sb.ToString();
        }
    }
    
    public class HyperContext : IHyperContext
    {
        public Locale Localization { get; set; }
        public IHyperOptions Options { get; set; }

        public HyperContext(Locale localization, IHyperOptions options = null)
        {
            Localization = localization;
            Options = options;
        }
    }

    public partial class Article(string key, IList<ISpecificationNode> nodes) : IArticle
    {
        public IList<ISpecificationNode> Nodes { get; set; } = nodes;
        public string Key { get; set; } = key;
    }
}
