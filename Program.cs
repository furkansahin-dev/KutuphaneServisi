using KutuphaneServisi;
using Microsoft.EntityFrameworkCore;
using KutuphaneServisi.Data;
using KutuphaneServisi.Repository;
using KutuphaneServisi.Service;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

// 3.5. Rate Limiting (İstek Sınırlandırma) Ayarları
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter(policyName: "fixed", fixedOptions =>
    {
        fixedOptions.PermitLimit = 4; // 10 saniyede en fazla 4 istek
        fixedOptions.Window = TimeSpan.FromSeconds(10); // Zaman aralığı 10 saniye
        fixedOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        fixedOptions.QueueLimit = 0; // Sınır aşılınca kuyruğa alma, direkt reddet
    });
});

var app = builder.Build();

app.UseRateLimiter();

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
