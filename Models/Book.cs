using System;

namespace KutuphaneServisi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty; // Kemal abinin istediği ek alan
        public int PageCount { get; set; }
        public DateTime PublishDate { get; set; } // Kemal abinin istediği ek alan
    }
}