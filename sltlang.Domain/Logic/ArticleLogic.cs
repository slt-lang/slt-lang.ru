using sltlang.Common.ArticleService.Models;
using sltlang.Domain.Ports;
using System.Xml.Linq;

namespace sltlang.Domain.Logic
{
    public class ArticleLogic(IHttpService httpService) : IArticleLogic
    {
        const string ArticleService = "ArticleService";

        public async Task<ArticleDto> GetArticle(string culture_key, string name)
        {
            var response = await httpService.JsonRequest<object, ArticleDto>(ArticleService, "GetArticle", [new("culture_key", culture_key), new("name", name)], HttpMethod.Get, null);
            return response.success ? response.response! : null!;
        }

        public async Task<ArticleDto[]> GetArticleHistory(string culture_key, string name)
        {
            var response = await httpService.JsonRequest<object, ArticleDto[]>(ArticleService, "GetArticleHistory", [new("culture_key", culture_key), new("name", name)], HttpMethod.Get, null);
            return response.success ? response.response! : null!;
        }

        public async Task UpsertArticle(ArticleDto article)
        {
            await httpService.JsonRequest<object, object>(ArticleService, "UpsertArticle", [], HttpMethod.Post, article);
        }

        public async Task RebaseArticle(int historyId)
        {
            await httpService.JsonRequest<object, object>(ArticleService, "RebaseArticle", [new("historyId", historyId)], HttpMethod.Post, null);
        }

        public async Task DeleteArticle(string culture_key, string name)
        {
            await httpService.JsonRequest<object, object>(ArticleService, "DeleteArticle", [new("culture_key", culture_key), new("name", name)], HttpMethod.Delete, null);
        }

        public async Task<ArticleDto[]> GetRating(string culture_key)
        {
            var response = await httpService.JsonRequest<object, ArticleDto[]>(ArticleService, "GetRating", [new("culture_key", culture_key)], HttpMethod.Get, null);
            return response.success ? response.response! : null!;
        }

        public async Task<ArticleDto[]> GetArticlesByUser(int userId)
        {
            var (success, response) = await httpService.JsonRequest<object, ArticleDto[]>(ArticleService, "GetUserEditions", [new("userId", userId)], HttpMethod.Get, null, TimeSpan.FromSeconds(3));
            return success ? response! : [];
        }
    }
}
