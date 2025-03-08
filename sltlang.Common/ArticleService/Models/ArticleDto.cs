using System;

namespace sltlang.Common.ArticleService.Models
{
    public class ArticleDto
    {
        public int Id { get; set; } = default!;
        public int HistoryId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Title { get; set; } = default!;
        public DateTime CreateDate { get; set; } = default!;
        public DateTime UpdateDate { get; set; } = default!;
        public string Content { get; set; } = default!;
    }
}
