using Microsoft.EntityFrameworkCore;
using PlataformaNoticias.Data;
using PlataformaNoticias.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Configurar DbContext con SQLite
builder.Services.AddDbContext<FeedbackContext>(options =>
    options.UseSqlite("Data Source=feedback.db"));

// Registrar servicio para consumir JSONPlaceholder (API externa)
builder.Services.AddHttpClient<IJsonPlaceholderService, JsonPlaceholderService>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});

// Registrar HttpClient para la API local de feedback
builder.Services.AddHttpClient("LocalApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5163/"); // Cambia el puerto si usas otro
});

var app = builder.Build();

// Ejecutar migraciones al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FeedbackContext>();
    db.Database.Migrate();
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PostMvc}/{action=Listar}/{id?}");

app.MapControllers();

app.Run();
