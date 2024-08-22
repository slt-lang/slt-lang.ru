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
        IHyperOptions Options { get; }
    }

    public interface ISpecificationNode
    {
        string View(IHyperContext context);
    }

    public interface ISpecification
    {
        public IList<ISpecificationNode> Nodes { get; }
        public string Generate(IHyperContext context)
        {
            var sb = new StringBuilder();
            foreach (var node in Nodes)
                sb.Append(node.View(context));
            return sb.ToString();
        }
    }

    public partial class Specification(IList<ISpecificationNode> nodes) : ISpecification
    {
        public IList<ISpecificationNode> Nodes { get; set; } = nodes;
    }
}
