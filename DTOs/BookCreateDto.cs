using System;

namespace KutuphaneServisi.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public DateTime PublishDate { get; set; }
        public int CategoryId { get; set; }
    }
}