using Microsoft.EntityFrameworkCore;
using KutuphaneServisi.Data;
using KutuphaneServisi.Repository;
using KutuphaneServisi.Service;

var builder = WebApplication.CreateBuilder(args);

// 1. Veri tabanı bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. SOLID Katmanlarımızın Tanıtılması
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<CategoryService>();

// 3. API ve Controller Desteğinin Eklenmesi
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. İstek Boru Hattı (Pipeline) Ayarları
if (app.Environment.IsDevelopment())
app.UseSwagger();
app.UseSwaggerUI();
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Controller dosyalarımızın dışarıya açılması
app.MapControllers();

app.Run();
// Final control completed.
