using System.Collections.Generic;

namespace KutuphaneServisi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // One-to-Many ilişkisi için: Bir kategorinin birden fazla kitabı olabilir
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}