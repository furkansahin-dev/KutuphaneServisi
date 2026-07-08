using KutuphaneServisi.Models;
using KutuphaneServisi.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutuphaneServisi.Service
{
    public class BookService
    {
        private readonly BookRepository _repository;

        public BookService(BookRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            // İleride buraya iş mantığı kuralları eklenebilir (Örn: sayfa sayısı kontrolü)
            await _repository.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _repository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _repository.GetByIdAsync(id);
            if (book != null)
            {
                await _repository.DeleteAsync(book);
            }
        }
    }
}