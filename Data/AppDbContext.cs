using Microsoft.EntityFrameworkCore;
using KutuphaneServisi.Models;

namespace KutuphaneServisi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Bu satır veri tabanında "Books" adında bir tablo oluşmasını sağlayacak
        public DbSet<Book> Books { get; set; }
    }
}