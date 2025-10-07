using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PARCIAL_programaci_n.Data
{
    // Esta clase es requerida por la herramienta 'dotnet ef' para crear el contexto
    // cuando se ejecutan comandos como 'dotnet ef migrations add' o 'dotnet ef database update'.
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Cambia "app.db" por el nombre de tu archivo de base de datos si es diferente
            optionsBuilder.UseSqlite("Data Source=app.db"); 

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}