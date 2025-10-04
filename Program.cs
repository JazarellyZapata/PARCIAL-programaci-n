using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PARCIAL_programaci_n.Data; // Incluye ApplicationUser y ApplicationDbContext
using PARCIAL_programaci_n.Models; // Incluye modelos si los necesitas en Program.cs, aunque no es estrictamente necesario aquí.

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE LA CONEXIÓN A LA BASE DE DATOS (CRÍTICO) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    // Asegúrate de que este método de conexión (UseSqlite) coincida con tu paquete NuGet
    options.UseSqlite(connectionString));


// --- 2. CONFIGURACIÓN DE IDENTITY Y ROLES (CRÍTICO) ---
// Configuración de Identity usando ApplicationUser, añadiendo soporte para ROLES
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // <-- ¡ESTA LÍNEA REGISTRA UserManager y RoleManager!
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Esta línea aplica TODAS las migraciones pendientes automáticamente al iniciar.
    // Solo se debe usar en entornos de desarrollo.
    dbContext.Database.Migrate(); 
}
// --- 3. CONFIGURACIÓN DE LA CADENA DE PETICIONES (Pipeline) ---

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Si estás en desarrollo, usa esta línea para que los errores de la DB sean visibles:
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Estaba faltando 'UseStaticFiles()'

app.UseRouting();

// Las llamadas a autenticación y autorización deben ir entre UseRouting() y MapControllerRoute()
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Añade el mapeo para las vistas de Identity (Login/Register)
app.MapRazorPages(); 

// --- CÓDIGO DE INICIALIZACIÓN DE ROLES (Seed Data) ---
// Este bloque es opcional pero muy recomendable para crear el rol "Coordinador"
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    // Crear rol "Coordinador" si no existe
    if (!await roleManager.RoleExistsAsync("Coordinador"))
    {
        await roleManager.CreateAsync(new IdentityRole("Coordinador"));
    }

    // Crear un usuario de prueba como Coordinador si no existe
    var coordinadorEmail = "coordinador@test.com";
    if (await userManager.FindByEmailAsync(coordinadorEmail) == null)
    {
        var coordinador = new ApplicationUser 
        { 
            UserName = coordinadorEmail, 
            Email = coordinadorEmail, 
            NombreCompleto = "Coordinador Principal",
            EmailConfirmed = true 
        };
        await userManager.CreateAsync(coordinador, "Password123!");
        await userManager.AddToRoleAsync(coordinador, "Coordinador");
    }
}


app.Run();