using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PARCIAL_programaci_n.Data; 
namespace PARCIAL_programaci_n.Models
{
    public class Asistencia
    {
        // Clave Primaria compuesta o simple, dependiendo de tu diseño
        [Key]
        public int Id { get; set; }

        // Indica si el estudiante estuvo presente (P4)
        [Display(Name = "Presente")]
        public bool Presente { get; set; } // <--- ¡Asegúrate de que esta línea exista!

        // Claves Foráneas
        
        [Required]
        public int SesionId { get; set; }
        [ForeignKey("SesionId")]
        public Sesion? Sesion { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;
        [ForeignKey("UsuarioId")]
        public ApplicationUser? Usuario { get; set; }
    }
}
