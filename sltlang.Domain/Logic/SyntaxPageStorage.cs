using sltlang.Domain.Models;
using sltlang.Domain.Ports;

namespace sltlang.Domain.Logic
{
    public class SyntaxPageStorage(ISyntaxPageMaker syntaxPageMaker, ILocaleService localeService) : ISyntaxPageStorage
    {
        private readonly SyntaxPage[] pages = syntaxPageMaker.Linking(
            localeService.Locales.Keys.SelectMany(x => syntaxPageMaker.PageFactory.Select(page => syntaxPageMaker.CreateSyntaxPage(page, x))).ToArray()
        );
        public IEnumerable<SyntaxPage> GetPages() => pages;
    }
}
