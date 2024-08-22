using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specification
{
    public partial class Article : IArticle
    {
        public static Dictionary<string, IArticle> ExecutablesSpecification = InitExecutablesSpecification();
        public static Dictionary<string, IArticle> OtherArticles = InitOtherArticles();

        public static Dictionary<string, IArticle> InitOtherArticles()
        {
            var dt = new Dictionary<string, IArticle>();
            return dt;
        }

        public static Dictionary<string, IArticle> InitExecutablesSpecification()
        {
            var dt = new List<IArticle>
            {
                    new Article("BaseExpression", [
                        new Tag.Paragraph("Str")
                    ]),
            };

            return dt.ToDictionary(x => x.Key.ToLower(), x => x);
        }
    }
}
