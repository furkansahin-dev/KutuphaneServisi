using KutuphaneServisi.Data;
using KutuphaneServisi.Models;
using Microsoft.EntityFrameworkCore; // İşte Include için eksik olan kütüphane buydu
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutuphaneServisi.Repository
{
    public class BookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        // Tüm kitapları listeleme (Read)
        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            // Kitapları çekerken bağlı olduğu kategori bilgisini de dahil ediyoruz
            return await _context.Books.Include(b => b.Category).ToListAsync();
        }

        // Id'ye göre tek bir kitap getirme (Read)
        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        // Yeni kitap ekleme (Create)
        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        // Kitap güncelleme (Update)
        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        // Kitap silme (Delete)
        public async Task DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}