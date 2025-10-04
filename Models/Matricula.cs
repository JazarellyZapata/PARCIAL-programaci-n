using System.ComponentModel.DataAnnotations;
// ¡IMPORTANTE! Agregado para ver ApplicationUser:
using PARCIAL_PROGRAMACI_N.Data;

// IMPORTANTE: El namespace debe coincidir con tu proyecto
namespace PARCIAL_PROGRAMACI_N.Models
{
    // Enumeración para el estado de la matrícula (P1)
    public enum EstadoMatricula
    {
        Pendiente,
        Confirmada,
        Cancelada
    }

    public class Matricula
    {
        // Clave Compuesta: { CursoId, UsuarioId }
        public int CursoId { get; set; }
        public string UsuarioId { get; set; } = string.Empty;

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        public EstadoMatricula Estado { get; set; } = EstadoMatricula.Pendiente;

        // Propiedades de navegación
        public Curso Curso { get; set; } = null!;
        // Referencia al usuario (necesita el using PARCIAL_PROGRAMACI_N.Data)
        public ApplicationUser? Usuario { get; set; }
    }
}