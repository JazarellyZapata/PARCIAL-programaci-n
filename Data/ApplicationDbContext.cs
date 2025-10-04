using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Para IdentityDbContext
using Microsoft.EntityFrameworkCore;                     // Para DbSet, ModelBuilder, etc.
using PARCIAL_programaci_n.Models;                   // Para Curso, Matricula, Sesion, Asistencia
using PARCIAL_programaci_n.Data;                     // Para ApplicationUser (Tu propio modelo)

namespace PARCIAL_programaci_n.Data 
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // ... Constructor ...

        // Nuevos DbSets para la Pregunta 4
        public DbSet<Sesion> Sesiones { get; set; } = null!;
        public DbSet<Asistencia> Asistencias { get; set; } = null!; // Añadir esta línea

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ... Configuración de Clave Compuesta Matricula (P3) ...

            // 1. Configuración de Clave Compuesta Única para Asistencia (P4)
            builder.Entity<Asistencia>()
                .HasKey(a => new { a.SesionId, a.UsuarioId });

            // 2. Configuración de relaciones para Asistencia
            builder.Entity<Asistencia>()
                .HasOne(a => a.Sesion)
                .WithMany(s => s.Asistencias)
                .HasForeignKey(a => a.SesionId);

            builder.Entity<Asistencia>()
                .HasOne(a => a.Usuario)
                .WithMany() // No es necesario una propiedad de navegación directa en ApplicationUser
                .HasForeignKey(a => a.UsuarioId);

            // ... El resto del código de Seeding (Roles, Usuario, Cursos) permanece igual ...
        }
    }
}