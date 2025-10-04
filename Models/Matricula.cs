using System.ComponentModel.DataAnnotations;

namespace PARCIAL_programaci_n.Models
{
    // Modelo de unión para la relación muchos a muchos (Curso y Usuario)
    public class Matricula
    {
        // Clave Compuesta: CursoId + UsuarioId (Definición en ApplicationDbContext)
        
        [Display(Name = "Curso")]
        public int CursoId { get; set; }
        public Curso? Curso { get; set; } // Navegación al curso

        [Display(Name = "Estudiante")]
        public string UsuarioId { get; set; } = string.Empty;
        public Data.ApplicationUser? Usuario { get; set; } // Navegación al usuario

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Matrícula")]
        public DateTime FechaMatricula { get; set; }
    }
}