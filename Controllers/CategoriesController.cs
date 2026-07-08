using Microsoft.AspNetCore.Mvc;
using KutuphaneServisi.Models;
using KutuphaneServisi.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KutuphaneServisi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // 1. Kategorileri Listele (GET: api/categories)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        // 2. ID'ye Göre Kategori Getir (GET: api/categories/{id})
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Kategori bulunamadı." });
            }
            return Ok(category);
        }

        // 3. Kategori Ekle (POST: api/categories)
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                return BadRequest(new { message = "Kategori adı boş olamaz." });
            }

            await _categoryService.AddCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // 4. Kategori Güncelle (PUT: api/categories/{id})
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest(new { message = "ID uyuşmazlığı." });
            }

            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Güncellenmek istenen kategori bulunamadı." });
            }

            await _categoryService.UpdateCategoryAsync(category);
            return NoContent();
        }

        // 5. Kategori Sil (DELETE: api/categories/{id})
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Silinmek istenen kategori bulunamadı." });
            }

            await _categoryService.DeleteCategoryAsync(existingCategory);
            return NoContent();
        }
    }
}