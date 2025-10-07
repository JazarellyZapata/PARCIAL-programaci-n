using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PARCIAL_programaci_n.Models; // <--- ¡AÑADE ESTA LÍNEA!
using System.Reflection.Emit;

namespace PARCIAL_programaci_n.Data
{
    // Heredamos de IdentityDbContext<ApplicationUser> para incluir las tablas de Identity
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Definición de las tablas (Modelos) del Parcial
        public DbSet<Curso> Cursos { get; set; } = default!;
        public DbSet<Matricula> Matriculas { get; set; } = default!;
        public DbSet<Sesion> Sesiones { get; set; } = default!;
        public DbSet<Asistencia> Asistencias { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Configuración de Claves Compuestas y Relaciones
            
            // 1. Clave compuesta para Matricula (para evitar duplicados en la matriculación)
            builder.Entity<Matricula>()
                .HasKey(m => new { m.CursoId, m.UsuarioId });

            // 2. Relación uno-a-muchos: Curso - Sesiones
            builder.Entity<Sesion>()
                .HasOne(s => s.Curso)
                .WithMany(c => c.Sesiones)
                .HasForeignKey(s => s.CursoId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra el curso, se borran las sesiones

            // 3. Relación uno-a-muchos: Sesion - Asistencias
            builder.Entity<Asistencia>()
                .HasOne(a => a.Sesion)
                .WithMany(s => s.Asistencias)
                .HasForeignKey(a => a.SesionId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra la sesión, se borran las asistencias

            // 4. Relación uno-a-muchos: Usuario - Asistencias
            builder.Entity<Asistencia>()
                .HasOne(a => a.Usuario)
                .WithMany() // No es necesario una colección de asistencias en ApplicationUser para el parcial
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrados en cascada no deseados
            
            // 5. Relación uno-a-muchos: Usuario - Matriculas
            builder.Entity<Matricula>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.Matriculas)
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}