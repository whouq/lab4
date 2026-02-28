using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавление контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление контроллеров и представлений
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Настройка конвейера обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=Index}/{id?}");

// Создание базы данных при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
