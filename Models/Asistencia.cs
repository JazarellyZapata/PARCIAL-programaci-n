using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PARCIAL_programaci_n.Data; // RESUELVE ApplicationUser

namespace PARCIAL_programaci_n.Models 
{
    public class Asistencia
    {
        // Clave Compuesta: SesionId + UsuarioId
        [Display(Name = "Sesión")]
        public int SesionId { get; set; }
        public Sesion? Sesion { get; set; }

        [Display(Name = "Estudiante")]
        public string UsuarioId { get; set; } = string.Empty;
        public ApplicationUser? Usuario { get; set; } // Enlace al usuario

        [Required]
        [Display(Name = "Asistió")]
        public bool Asistio { get; set; } = true;

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
    }
}