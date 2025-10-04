using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Necesario para IdentityRole
using PARCIAL_programaci_n.Data;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------------
// 1. CONFIGURACIÓN DE BASE DE DATOS Y AUTENTICACIÓN (CRUCIAL PARA P1/P2/P3)
// ----------------------------------------------------------------------

// 1.1. Obtener la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 1.2. Configurar ApplicationDbContext para usar SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString)); 

// 1.3. Habilitar el filtro para excepciones de base de datos en desarrollo
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 1.4. Configurar Identity con ApplicationUser, Roles y ApplicationDbContext
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // IMPORTANTE: Agrega soporte para Roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

// ----------------------------------------------------------------------
// 2. CONFIGURACIÓN DE MVC Y VISTAS
// ----------------------------------------------------------------------

// Add services to the container.
builder.Services.AddControllersWithViews();
// builder.Services.AddRazorPages(); // Necesario si usas el área de Identity

var app = builder.Build();

// ----------------------------------------------------------------------
// 3. CONFIGURACIÓN DEL PIPELINE DE SOLICITUDES
// ----------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Usa el filtro de excepciones para la base de datos solo en desarrollo
    app.UseMigrationsEndPoint(); 
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Necesario para CSS/JS

app.UseRouting();

// 3.1. Habilitar Autenticación y Autorización
app.UseAuthentication(); 
app.UseAuthorization();

// app.MapRazorPages(); // Necesario si usas el área de Identity.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();