using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PARCIAL_programaci_n.Data; // RESUELVE ApplicationUser

namespace PARCIAL_programaci_n.Models 
{
    public class Sesion
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Curso")]
        public int CursoId { get; set; }
        [ForeignKey("CursoId")]
        public Curso? Curso { get; set; } // Enlace al modelo Curso

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Sesión")]
        public DateTime FechaSesion { get; set; }

        [StringLength(255)]
        [Display(Name = "Tema Cubierto")]
        public string? Tema { get; set; }

        // Propiedad de navegación para el registro de asistencia
        public ICollection<Asistencia>? Asistencias { get; set; }
    }
}