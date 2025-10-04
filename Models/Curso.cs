using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

// IMPORTANTE: El namespace debe coincidir con tu proyecto
namespace PARCIAL_PROGRAMACI_N.Models
{
    public class Curso
    {
        // Propiedades de la Entidad
        public int Id { get; set; }

        [Required(ErrorMessage = "El código del curso es obligatorio.")]
        [MaxLength(10)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty; // Único

        [Required(ErrorMessage = "El nombre del curso es obligatorio.")]
        [Display(Name = "Nombre del Curso")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los créditos son obligatorios.")]
        [Range(1, 10, ErrorMessage = "Los créditos deben ser mayores que cero y no exceder 10.")]
        public int Creditos { get; set; } 

        [Required(ErrorMessage = "El cupo máximo es obligatorio.")]
        [Display(Name = "Cupo Máximo")]
        [Range(1, 100, ErrorMessage = "El cupo debe ser al menos 1.")]
        public int CupoMaximo { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [Display(Name = "Inicio")]
        [DataType(DataType.Time)]
        public TimeSpan HorarioInicio { get; set; }

        [Required(ErrorMessage = "La hora de finalización es obligatoria.")]
        [Display(Name = "Fin")]
        [DataType(DataType.Time)]
        public TimeSpan HorarioFin { get; set; } 

        public bool Activo { get; set; } = true;

        // Propiedad de navegación (Colección de matrículas asociadas a este curso)
        public ICollection<Matricula>? Matriculas { get; set; }

        // Propiedad calculada para el estado actual del cupo (P3)
        [NotMapped]
        public int CupoActual => Matriculas?.Count(m => m.Estado == EstadoMatricula.Confirmada) ?? 0;
        
        [NotMapped]
        public int CupoDisponible => CupoMaximo - CupoActual;
    }
}