using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PARCIAL_programaci_n.Models; // Para acceder a Curso y Matricula

namespace PARCIAL_programaci_n.Data 
{
    // Hereda de IdentityDbContext<ApplicationUser> para incluir las tablas de Identity
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Definición de los DbSets de tus modelos de dominio (P1/P2)
        public DbSet<Curso> Cursos { get; set; } = null!;
        public DbSet<Matricula> Matriculas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Llama a la implementación base para configurar Identity
            base.OnModelCreating(builder);

            // 1. Restricción de Clave Compuesta Única para Matricula (P1)
            builder.Entity<Matricula>()
                .HasKey(m => new { m.CursoId, m.UsuarioId });

            // 2. Restricción de unicidad para el Código de Curso (P1)
            builder.Entity<Curso>()
                .HasIndex(c => c.Codigo)
                .IsUnique();

            // 3. Inserción Inicial de Datos (Seeding)
            SeedData(builder);
        }

        private void SeedData(ModelBuilder builder)
        {
            // **GUIDS ESTÁTICOS** para evitar el error 'PendingModelChangesWarning'
            var coordinadorRoleId = "d608670a-42c6-4320-b302-601831c19b62"; 
            var userId = "b94c32b5-3135-4603-b0e6-e910229381c1"; 

            // 3.1. Rol Coordinador
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = coordinadorRoleId,
                Name = "Coordinador",
                NormalizedName = "COORDINADOR"
            });

            // 3.2. Usuario Inicial (jazarelly_zapata@usmp.pe / programacion)
            var user = new ApplicationUser
            {
                Id = userId,
                // Nuevas Credenciales Solicitadas:
                UserName = "jazarelly_zapata@usmp.pe",
                NormalizedUserName = "JAZARELLY_ZAPATA@USMP.PE",
                Email = "jazarelly_zapata@usmp.pe",
                NormalizedEmail = "JAZARELLY_ZAPATA@USMP.PE",
                EmailConfirmed = true
            };
            
            // Crea el hash de la contraseña "programacion"
            var hasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = hasher.HashPassword(user, "programacion");

            builder.Entity<ApplicationUser>().HasData(user);

            // 3.3. Asignar el rol Coordinador al usuario
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = coordinadorRoleId,
                UserId = user.Id
            });

            // 3.4. Cursos Iniciales
            builder.Entity<Curso>().HasData(
                new Curso { Id = 1, Codigo = "PROG101", Nombre = "Introducción a la Programación", Creditos = 4, CupoMaximo = 25, HorarioInicio = new TimeSpan(9, 0, 0), HorarioFin = new TimeSpan(11, 0, 0), Activo = true },
                new Curso { Id = 2, Codigo = "WEB202", Nombre = "Desarrollo Web Full-Stack", Creditos = 5, CupoMaximo = 15, HorarioInicio = new TimeSpan(14, 0, 0), HorarioFin = new TimeSpan(17, 0, 0), Activo = true },
                new Curso { Id = 3, Codigo = "BD303", Nombre = "Bases de Datos Avanzadas", Creditos = 3, CupoMaximo = 30, HorarioInicio = new TimeSpan(11, 0, 0), HorarioFin = new TimeSpan(13, 0, 0), Activo = true }
            );
        }
    }
}