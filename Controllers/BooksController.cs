using Microsoft.AspNetCore.Mvc;
using KutuphaneServisi.Models;
using KutuphaneServisi.Service;
using KutuphaneServisi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using AutoMapper; // AutoMapper kütüphanesini ekledik

namespace KutuphaneServisi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.RateLimiting.EnableRateLimiting("fixed")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly CategoryService _categoryService;
        private readonly IMapper _mapper; // IMapper tanımı eklendi

        // Constructor üzerinden IMapper inject edildi
        public BooksController(BookService bookService, CategoryService categoryService, IMapper mapper)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _mapper = mapper;
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

            // AutoMapper ile toplu dönüşüm (Manuel Select temizlendi)
            var bookDtos = _mapper.Map<List<BookDto>>(books);
            
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

            // AutoMapper ile tekil dönüşüm yapıldı
            var bookDto = _mapper.Map<BookDto>(book);

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

            // AutoMapper ile BookCreateDto -> Book dönüşümü yapıldı
            var book = _mapper.Map<Book>(bookCreateDto);

            await _bookService.AddBookAsync(book);

            // Geri dönüş tipi için eşleştirme yapılıyor
            var resultDto = _mapper.Map<BookDto>(book);
            resultDto.CategoryName = category.Name; // Kategori adını doğrudan atıyoruz

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

            // Manuel atamalar yerine AutoMapper nesne üzerine eşleme (Map) yapıyor
            _mapper.Map(bookUpdateDto, existingBook);

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