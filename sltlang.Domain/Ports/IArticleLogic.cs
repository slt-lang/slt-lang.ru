using sltlang.Common.ArticleService.Models;

namespace sltlang.Domain.Ports
{
    public interface IArticleLogic
    {
        Task<ArticleDto> GetArticle(string culture_key, string name);
        Task<ArticleDto[]> GetArticleHistory(string culture_key, string name);
        Task UpsertArticle(ArticleDto article);
        Task RebaseArticle(int historyId);
        Task DeleteArticle(string culture_key, string name);
        Task<ArticleDto[]> GetRating(string culture_key);
    }
}
