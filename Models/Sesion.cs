using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PARCIAL_programaci_n.Models
{
    public class Sesion
    {
        // Clave Primaria
        [Key]
        public int Id { get; set; }

        // Propiedades de la Sesión

        [Required(ErrorMessage = "La fecha y hora son obligatorias.")]
        [Display(Name = "Fecha y Hora")]
        // **IMPORTANTE: Usamos 'Fecha' para coincidir con la corrección de la vista.**
        public DateTime Fecha { get; set; } 

        [Required(ErrorMessage = "El tema es obligatorio.")]
        [StringLength(200)]
        public string Tema { get; set; } = string.Empty;

        // Relación con Curso (Clave Foránea)

        [Required(ErrorMessage = "El curso es obligatorio.")]
        public int CursoId { get; set; }

        [ForeignKey("CursoId")]
        // Propiedad de navegación hacia el Curso
        public Curso? Curso { get; set; }

        // Relación con Asistencia (Colección de asistencias para esta sesión)

        // Propiedad de navegación hacia Asistencias (uno a muchos)
        public ICollection<Asistencia>? Asistencias { get; set; }
    }
}