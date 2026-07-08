using Microsoft.AspNetCore.Mvc;
using KutuphaneServisi.Models;
using KutuphaneServisi.Service;
using KutuphaneServisi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace KutuphaneServisi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly CategoryService _categoryService;

        public BooksController(BookService bookService, CategoryService categoryService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] string? title, [FromQuery] int? categoryId)
        {
            var books = await _bookService.GetAllBooksAsync();
            
            // Başlığa göre filtreleme
            if (!string.IsNullOrEmpty(title))
            {
                books = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }

            // Kategoriye göre filtreleme
            if (categoryId.HasValue)
            {
                books = books.Where(b => b.CategoryId == categoryId.Value);
            }

            // Verileri DTO formatına dönüştürme (Mapping)
            var bookDtos = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Publisher = b.Publisher,
                PageCount = b.PageCount,
                PublishDate = b.PublishDate,
                CategoryId = b.CategoryId,
                CategoryName = b.Category != null ? b.Category.Name : "Kategori Yok"
            }).ToList();
            
            return Ok(bookDtos);
        }

        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var books = await _bookService.GetAllBooksAsync();
            var book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound(new { message = "Kitap bulunamadı." });
            }

            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                PageCount = book.PageCount,
                PublishDate = book.PublishDate,
                CategoryId = book.CategoryId,
                CategoryName = book.Category != null ? book.Category.Name : "Kategori Yok"
            };

            return Ok(bookDto);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<BookDto>> PostBook(BookCreateDto bookCreateDto)
        {
            // İlişkili kategori kontrolü
            var category = await _categoryService.GetCategoryByIdAsync(bookCreateDto.CategoryId);
            if (category == null)
            {
                return BadRequest(new { message = $"Hata: {bookCreateDto.CategoryId} ID'li bir kategori bulunamadı!" });
            }

            if (bookCreateDto.PageCount <= 0)
            {
                return BadRequest(new { message = "Sayfa sayısı 0 veya daha küçük olamaz." });
            }

            var book = new Book
            {
                Title = bookCreateDto.Title,
                Author = bookCreateDto.Author,
                Publisher = bookCreateDto.Publisher,
                PageCount = bookCreateDto.PageCount,
                PublishDate = bookCreateDto.PublishDate,
                CategoryId = bookCreateDto.CategoryId
            };

            await _bookService.AddBookAsync(book);

            var resultDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Publisher = book.Publisher,
                PageCount = book.PageCount,
                PublishDate = book.PublishDate,
                CategoryId = book.CategoryId,
                CategoryName = category.Name
            };

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, resultDto);
        }

        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookCreateDto bookUpdateDto)
        {
            var existingBook = await _bookService.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound(new { message = "Güncellenmek istenen kitap bulunamadı." });
            }

            var category = await _categoryService.GetCategoryByIdAsync(bookUpdateDto.CategoryId);
            if (category == null)
            {
                return BadRequest(new { message = $"Hata: {bookUpdateDto.CategoryId} ID'li bir kategori bulunamadı!" });
            }

            existingBook.Title = bookUpdateDto.Title;
            existingBook.Author = bookUpdateDto.Author;
            existingBook.Publisher = bookUpdateDto.Publisher;
            existingBook.PageCount = bookUpdateDto.PageCount;
            existingBook.PublishDate = bookUpdateDto.PublishDate;
            existingBook.CategoryId = bookUpdateDto.CategoryId;

            await _bookService.UpdateBookAsync(existingBook);
            return NoContent();
        }

        // DELETE: api/books/{id}
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