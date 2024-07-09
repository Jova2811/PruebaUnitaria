using Microsoft.EntityFrameworkCore;
using PruebaUnitaria.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión para DbPracticaContext
var connectionString = builder.Configuration.GetConnectionString("PruebaUnitariaContext") ??
    throw new InvalidOperationException("Connection string 'PruebaUnitariaContext' not found.");
builder.Services.AddDbContext<DbPracticaContext>(options =>
    options.UseSqlServer(connectionString));

// Agregar servicios a la contenedor de dependencias
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar el pipeline HTTP
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
