using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PARCIAL_programaci_n.Data; // Necesario para ApplicationUser

namespace PARCIAL_programaci_n.Models
{
    public class Matricula
    {
        // Esta combinación será definida como clave compuesta en ApplicationDbContext
        
        [Display(Name = "Curso")]
        public int CursoId { get; set; }
        [ForeignKey("CursoId")]
        public Curso? Curso { get; set; }

        [Display(Name = "Estudiante")]
        public string UsuarioId { get; set; } = string.Empty;
        [ForeignKey("UsuarioId")]
        public ApplicationUser? Usuario { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Matrícula")]
        public DateTime FechaMatricula { get; set; } = DateTime.Now;
    }
}