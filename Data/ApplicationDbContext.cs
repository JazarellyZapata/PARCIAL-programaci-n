using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // RESUELVE IdentityDbContext
using Microsoft.EntityFrameworkCore;                     // RESUELVE DbSet, ModelBuilder, DbContextOptions
using PARCIAL_programaci_n.Models;                   // RESUELVE Curso, Matricula, Sesion, Asistencia
using PARCIAL_programaci_n.Data;                     // RESUELVE ApplicationUser

namespace PARCIAL_programaci_n.Data 
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets de la Pregunta 2 y 3
        public DbSet<Curso> Cursos { get; set; } = null!;
        public DbSet<Matricula> Matriculas { get; set; } = null!;

        // DbSets de la Pregunta 4 y 5
        public DbSet<Sesion> Sesiones { get; set; } = null!;
        public DbSet<Asistencia> Asistencias { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de Clave Compuesta Única para Matricula (P3)
            builder.Entity<Matricula>()
                .HasKey(m => new { m.CursoId, m.UsuarioId });

            // 1. Configuración de Clave Compuesta Única para Asistencia (P4/P5)
            builder.Entity<Asistencia>()
                .HasKey(a => new { a.SesionId, a.UsuarioId });

            // 2. Configuración de relaciones para Asistencia
            builder.Entity<Asistencia>()
                .HasOne(a => a.Sesion)
                .WithMany(s => s.Asistencias)
                .HasForeignKey(a => a.SesionId);

            builder.Entity<Asistencia>()
                .HasOne(a => a.Usuario)
                .WithMany() // Propiedad de navegación en ApplicationUser no es estrictamente necesaria aquí
                .HasForeignKey(a => a.UsuarioId);

            // Seeding de Datos (Omitido para brevedad, asumo que ya lo tienes)
            // ... (Código de Roles, ApplicationUser, y Cursos) ... 
        }
    }
}