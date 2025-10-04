using System.ComponentModel.DataAnnotations;

namespace PARCIAL_programaci_n.Models
{
    // Modelo fundamental para la Pregunta 2 (Gestión) y Pregunta 3 (Matrícula)
    public class Curso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(10, ErrorMessage = "El código no puede exceder los 10 caracteres.")]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        [Display(Name = "Nombre del Curso")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los créditos son obligatorios.")]
        [Range(1, 10, ErrorMessage = "Los créditos deben estar entre 1 y 10.")]
        public int Creditos { get; set; }

        [Required(ErrorMessage = "El cupo máximo es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El cupo máximo debe estar entre 1 y 100.")]
        [Display(Name = "Cupo Máximo")]
        public int CupoMaximo { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [DataType(DataType.Time)]
        [Display(Name = "Horario Inicio")]
        public TimeSpan HorarioInicio { get; set; }

        [Required(ErrorMessage = "La hora de fin es obligatoria.")]
        [DataType(DataType.Time)]
        [Display(Name = "Horario Fin")]
        public TimeSpan HorarioFin { get; set; }

        [Display(Name = "¿Está Activo?")]
        public bool Activo { get; set; } = true;

        // Propiedad de navegación para Matriculas (Crucial para P3: permite contar el cupo)
        public ICollection<Matricula>? Matriculas { get; set; }
    }
}