using SLThree;
using SLThree.Extensions;
using SLThree.Metadata;
using SLThree.Visitors;
using sltlang.Domain;
using sltlang.Domain.Models;
using sltlang.Domain.Ports;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace sltlang.Adapters.Adapters
{
    public class SampleMaker(ILanguageProvider languageProvider, SLThreeHtmlRestorator htmlRestorator) : ISampleMaker
    {
        public static readonly Type[] ListedTypes = typeof(SLThree.ExecutionContext).Assembly.GetTypes().Where(x => x.IsPublic || x.IsNestedPublic).ToArray();

        public IEnumerable<(string FileName, string Code)> CodeFactory
        {
            get
            {
                var assembly = typeof(SampleMaker).Assembly;
                var resources = assembly.GetManifestResourceNames().Where(x => x.Contains(".samples.") && x.EndsWith(".slt")).ToArray();
                return resources.Select(x => (x.Replace("sltlang.Adapters.samples.", ""), assembly.GetManifestResourceStream(x).ReadString()));
            }
        }

        public CodeSample CreateSample((string FileName, string Code) res)
        {
            var statements = languageProvider.Parser.ParseScript(res.Code, res.FileName);
            var metadata = GetMetadataContext(statements as StatementList);
            (statements as StatementList)!.Statements = (statements as StatementList)!.Statements.Where(x => !MetadataPredicate(x)).ToArray();

            var ret = new CodeSample();
            ret.Title = metadata?["Title"]?.ToString()!;
            ret.Description = metadata?["Description"]?.ToString()!;
            ret.CultureKey = metadata?["CultureKey"]?.ToString() ?? MultiCultureKey.Value;
            ret.LineComments = metadata?["LineComments"] as Dictionary<object, object>;
            //line comments here from metadata

            var typeCounter = new TypeCounter();
            typeCounter.VisitAny(statements);
            ret.CodeStatistic = new ConcurrentDictionary<Type, double>(typeCounter.Count.ToDictionary(x => x.Key, x => x.Value / (double)typeCounter.Overall));

            var html = htmlRestorator.Restore(statements);
            ret.ReadyHtml = html;

            return ret;
        }

        private class TypeCounter : AbstractVisitor
        {
            public TypeCounter()
            {
                Count = ListedTypes.ToDictionary(x => x, x => 0);
            }

            public int Overall { get; set; }
            public Dictionary<Type, int> Count { get; set; }

            public override void VisitExpression(BaseExpression expression)
            {
                var type = expression.GetType();
                foreach (var x in ListedTypes)
                {
                    if (type.IsType(x)) Count[x] += 1;
                }
                Overall += 1;
                base.VisitExpression(expression);
            }
            public override void VisitStatement(BaseStatement statement)
            {
                var type = statement.GetType();
                foreach (var x in ListedTypes)
                {
                    if (type.IsType(x)) Count[x] += 1;
                }
                Overall += 1;
                base.VisitStatement(statement);
            }
        }

        private static Func<BaseStatement, bool> MetadataPredicate = x => 
            x is ExpressionStatement expr && expr.Expression is CreatorContext crctx && crctx.Name is NameExpression name && name.Name == "メタデータ";
        private static ContextWrap? GetMetadataContext(StatementList? statements)
        {
            var expr = statements?.Statements.FirstOrDefault(MetadataPredicate);
            return ((expr as ExpressionStatement).Expression as CreatorContext).GetValue(new SLThree.ExecutionContext()) as ContextWrap;
        }
    }
}
