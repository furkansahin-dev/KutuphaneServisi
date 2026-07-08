using Microsoft.AspNetCore.Mvc;
using KutuphaneServisi.Models;
using KutuphaneServisi.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutuphaneServisi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        // 1. Tüm Kitapları Listele (GET: api/books)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        // 2. ID'ye Göre Kitap Getir (GET: api/books/{id})
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Kitap bulunamadı." });
            }
            return Ok(book);
        }

        // 3. Yeni Kitap Ekle (POST: api/books)
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            if (book.PageCount <= 0)
            {
                return BadRequest(new { message = "Sayfa sayısı 0 veya daha küçük olamaz." });
            }

            await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // 4. Kitap Güncelle (PUT: api/books/{id})
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest(new { message = "ID uyuşmazlığı." });
            }

            var existingBook = await _bookService.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound(new { message = "Güncellenmek istenen kitap bulunamadı." });
            }

            await _bookService.UpdateBookAsync(book);
            return NoContent();
        }

        // 5. Kitap Sil (DELETE: api/books/{id})
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var existingBook = await _bookService.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound(new { message = "Silinmek istenen kitap bulunamadı." });
            }

            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}