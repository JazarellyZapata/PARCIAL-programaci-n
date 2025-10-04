using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PARCIAL_PROGRAMACI_N.Models; // Necesario para ver Curso y Matricula
using System;
using System.Security.Claims;

// IMPORTANTE: El namespace debe coincidir con tu proyecto
namespace PARCIAL_PROGRAMACI_N.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Definición de las tablas del Dominio (P1)
        public DbSet<Curso> Cursos { get; set; } = null!;
        public DbSet<Matricula> Matriculas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Restricción de Clave Compuesta Única para Matrícula (P1)
            builder.Entity<Matricula>()
                .HasKey(m => new { m.CursoId, m.UsuarioId });

            // 2. Restricción de unicidad para el Código de Curso (P1)
            builder.Entity<Curso>()
                .HasIndex(c => c.Codigo)
                .IsUnique();

            // 3. Seeding Inicial de Datos (P1)
            
            // USAMOS GUIDS ESTÁTICOS Y FIJOS PARA EVITAR EL ERROR DE "PENDING MODEL CHANGES"

            // 3.1. Rol Coordinador
            var coordinadorRoleId = "d608670a-42c6-4320-b302-601831c19b62"; // GUID FIJO
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = coordinadorRoleId,
                Name = "Coordinador",
                NormalizedName = "COORDINADOR"
            });

            // 3.2. Usuario Inicial (coordinador@portal.uni / P@$$w0rd123)
            var user = new ApplicationUser
            {
                Id = "b94c32b5-3135-4603-b0e6-e910229381c1", // GUID FIJO
                UserName = "coordinador@portal.uni",
                NormalizedUserName = "COORDINADOR@PORTAL.UNI",
                Email = "coordinador@portal.uni",
                NormalizedEmail = "COORDINADOR@PORTAL.UNI",
                EmailConfirmed = true
            };
            
            // Genera el hash de la contraseña 'P@$$w0rd123'
            var hasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = hasher.HashPassword(user, "P@$$w0rd123");

            builder.Entity<ApplicationUser>().HasData(user);

            // 3.3. Asignar el rol Coordinador al usuario
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = coordinadorRoleId,
                UserId = user.Id
            });

            // 3.4. Cursos Iniciales (P1)
            builder.Entity<Curso>().HasData(
                new Curso
                {
                    Id = 1,
                    Codigo = "PROG101",
                    Nombre = "Introducción a la Programación",
                    Creditos = 4,
                    CupoMaximo = 25,
                    HorarioInicio = new TimeSpan(9, 0, 0),
                    HorarioFin = new TimeSpan(11, 0, 0),
                    Activo = true
                },
                new Curso
                {
                    Id = 2,
                    Codigo = "WEB202",
                    Nombre = "Desarrollo Web Full-Stack",
                    Creditos = 5,
                    CupoMaximo = 15,
                    HorarioInicio = new TimeSpan(14, 0, 0),
                    HorarioFin = new TimeSpan(17, 0, 0),
                    Activo = true
                },
                new Curso
                {
                    Id = 3,
                    Codigo = "BD303",
                    Nombre = "Bases de Datos Avanzadas",
                    Creditos = 3,
                    CupoMaximo = 30,
                    HorarioInicio = new TimeSpan(11, 0, 0),
                    HorarioFin = new TimeSpan(13, 0, 0),
                    Activo = true
                }
            );
        }
    }
}