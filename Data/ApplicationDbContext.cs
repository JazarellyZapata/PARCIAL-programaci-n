using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PARCIAL_programaci_n.Models;

namespace PARCIAL_programaci_n.Data 
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets necesarios para la P2 y P3
        public DbSet<Curso> Cursos { get; set; } = null!;
        public DbSet<Matricula> Matriculas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // NECESARIO: Llama a la implementación base para configurar Identity
            base.OnModelCreating(builder);

            // 1. Configuración de Clave Compuesta Única para Matricula (P1/P3)
            builder.Entity<Matricula>()
                .HasKey(m => new { m.CursoId, m.UsuarioId });

            // 2. Restricción de unicidad para el Código de Curso (P2)
            builder.Entity<Curso>()
                .HasIndex(c => c.Codigo)
                .IsUnique();
            
            // 3. Configuración de relaciones para Matricula (P3)
            builder.Entity<Matricula>()
                .HasOne(m => m.Curso)
                .WithMany(c => c.Matriculas)
                .HasForeignKey(m => m.CursoId);

            builder.Entity<Matricula>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.Matriculas)
                .HasForeignKey(m => m.UsuarioId);


            // 4. Inserción Inicial de Datos (Seeding)
            // Utiliza GUIDS y Tiempos ESTÁTICOS para evitar errores de migración
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // GUIDS ESTÁTICOS
            var coordinadorRoleId = "d608670a-42c6-4320-b302-601831c19b62"; 
            var estudianteRoleId = "a3f5a28f-7467-4630-9b43-41c10d3f20a9"; 
            var userId = "b94c32b5-3135-4603-b0e6-e910229381c1";
            
            // 4.1. Roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = coordinadorRoleId, Name = "Coordinador", NormalizedName = "COORDINADOR" },
                new IdentityRole { Id = estudianteRoleId, Name = "Estudiante", NormalizedName = "ESTUDIANTE" }
            );

            // 4.2. Usuario Coordinador (jazarelly_zapata@usmp.pe / programacion)
            var hasher = new PasswordHasher<ApplicationUser>();
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "jazarelly_zapata@usmp.pe", 
                NormalizedUserName = "JAZARELLY_ZAPATA@USMP.PE",
                Email = "jazarelly_zapata@usmp.pe", 
                NormalizedEmail = "JAZARELLY_ZAPATA@USMP.PE",
                EmailConfirmed = true,
                NombreCompleto = "Jazarelly Zapata",
                PasswordHash = hasher.HashPassword(null, "programacion") 
            };

            builder.Entity<ApplicationUser>().HasData(user);

            // 4.3. Asignar Rol
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = coordinadorRoleId, UserId = userId }
            );
            
            // 4.4. Cursos Iniciales
            builder.Entity<Curso>().HasData(
                new Curso { Id = 1, Codigo = "PROG101", Nombre = "Introducción a la Programación", Creditos = 4, CupoMaximo = 25, HorarioInicio = new TimeSpan(9, 0, 0), HorarioFin = new TimeSpan(11, 0, 0), Activo = true },
                new Curso { Id = 2, Codigo = "WEB202", Nombre = "Desarrollo Web Full-Stack", Creditos = 5, CupoMaximo = 15, HorarioInicio = new TimeSpan(14, 0, 0), HorarioFin = new TimeSpan(17, 0, 0), Activo = true },
                new Curso { Id = 3, Codigo = "BD303", Nombre = "Bases de Datos Avanzadas", Creditos = 3, CupoMaximo = 30, HorarioInicio = new TimeSpan(11, 0, 0), HorarioFin = new TimeSpan(13, 0, 0), Activo = true }
            );
        }
    }
}