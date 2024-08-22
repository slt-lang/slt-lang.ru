using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specification
{
    public partial class Specification
    {
        public static Dictionary<Type, Specification> ExecutablesSpecification = InitExecutablesSpecification();

        public static Dictionary<Type, Specification> InitExecutablesSpecification()
        {
            var dt = new Dictionary<Type, Specification>
            {
                {
                    typeof(SLThree.BaseExpression),
                    new Specification([
                        new Tag.Paragraph("Str")
                    ])
                }
            };

            return dt;
        }
    }
}
