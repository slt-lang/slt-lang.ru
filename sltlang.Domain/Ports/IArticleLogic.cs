using sltlang.Common.ArticleService.Models;

namespace sltlang.Domain.Ports
{
    public interface IArticleLogic
    {
        Task<ArticleDto> GetArticle(string name);
        Task<ArticleDto[]> GetArticleHistory(string name);
        Task UpsertArticle(ArticleDto article);
        Task RebaseArticle(int historyId);
        Task DeleteArticle(string name);
        Task<ArticleDto[]> GetRating();
    }
}
