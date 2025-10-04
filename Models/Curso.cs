using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PARCIAL_programaci_n.Models 
{
    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [MaxLength(10)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los créditos son obligatorios.")]
        [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10.")]
        public int Creditos { get; set; }

        [Required(ErrorMessage = "El cupo máximo es obligatorio.")]
        [Range(5, 50, ErrorMessage = "El cupo máximo debe estar entre 5 y 50.")]
        [Display(Name = "Cupo Máximo")]
        public int CupoMaximo { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [Display(Name = "Hora de Inicio")]
        public TimeSpan HorarioInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria.")]
        [Display(Name = "Hora de Fin")]
        public TimeSpan HorarioFin { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        // Propiedad de Navegación
        public ICollection<Matricula>? Matriculas { get; set; }
    }
}