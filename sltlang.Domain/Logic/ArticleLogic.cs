using sltlang.Common.ArticleService.Models;
using sltlang.Domain.Ports;
using System.Xml.Linq;

namespace sltlang.Domain.Logic
{
    public class ArticleLogic(IHttpService httpService) : IArticleLogic
    {
        const string ArticleService = "ArticleService";

        public async Task<ArticleDto> GetArticle(string name)
        {
            var response = await httpService.JsonRequest<object, ArticleDto>(ArticleService, $"GetArticle?name={name}", HttpMethod.Get, null);
            return response.success ? response.response! : null!;
        }

        public async Task<ArticleDto[]> GetArticleHistory(string name)
        {
            var response = await httpService.JsonRequest<object, ArticleDto[]>(ArticleService, $"GetArticleHistory?name={name}", HttpMethod.Get, null);
            return response.success ? response.response! : null!;
        }

        public async Task UpsertArticle(ArticleDto article)
        {
            await httpService.JsonRequest<object, object>(ArticleService, "UpsertArticle", HttpMethod.Post, article);
        }

        public async Task RebaseArticle(int historyId)
        {
            await httpService.JsonRequest<object, object>(ArticleService, $"RebaseArticle?historyId={historyId}", HttpMethod.Post, null);
        }

        public async Task DeleteArticle(string name)
        {
            await httpService.JsonRequest<object, object>(ArticleService, $"DeleteArticle?name={name}", HttpMethod.Delete, null);
        }
    }
}
