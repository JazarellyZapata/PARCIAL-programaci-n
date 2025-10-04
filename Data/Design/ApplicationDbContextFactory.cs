using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PARCIAL_programaci_n.Data; // Asegúrate de que este namespace es correcto

namespace PARCIAL_programaci_n.Data.Design
{
    // Implementa la interfaz IDesignTimeDbContextFactory
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Define la cadena de conexión de desarrollo para SQLite (la base de datos se crea en la raíz del proyecto)
            // ASEGÚRATE DE QUE ESTO COINCIDE CON TU CONFIGURACIÓN (generalmente SQLite)
            optionsBuilder.UseSqlite("Data Source=app.db"); 

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}