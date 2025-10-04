using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PARCIAL_programaci_n.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Código")]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public int Creditos { get; set; }

        [Required]
        [Display(Name = "Cupo Máximo")]
        public int CupoMaximo { get; set; }

        [Display(Name = "Estado")]
        public bool Activo { get; set; } = true; // Por defecto Activo

        // Propiedades de navegación (necesarias para P3/P5)
        public ICollection<Matricula>? Matriculas { get; set; }
        public ICollection<Sesion>? Sesiones { get; set; }
    }
}